using System.Diagnostics;
using System.IO;
using UnityEngine;
using TMPro;
using System.Collections;
using SimpleFileBrowser;
using System.Threading.Tasks;
public class PythonScriptRunner : MonoBehaviour
{
    private string scriptPath;
    private string jsonFilePath; 
    private string modelPath ; 

    [SerializeField] private TMP_Text pythonOutput;
    [SerializeField] private TMP_Text lastPythonOutput;

    [SerializeField] private TMP_Text loadingText;  

    private Coroutine loadingCoroutine; 

    private static PythonScriptRunner _instance;
    public static PythonScriptRunner Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void SelectAiModel()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("AIModels", ".pth"));

        FileBrowser.SetDefaultFilter(".pth");

        FileBrowser.AddQuickLink("AIModels", Application.persistentDataPath.ToString(), null);

        _ = SaveManager.Instance.SaveAsync("_ModelForClassify");
        StartCoroutine(ShowLoadDialogCoroutine());

    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        GameStateManager.Instance.ChangeState("LoadState");

        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, Application.persistentDataPath.ToString(), null, "Select Files", "Load");

        if (FileBrowser.Success)
            OnFilesSelected(FileBrowser.Result);
        GameStateManager.Instance.ChangeState("Game");


    }

    void OnFilesSelected(string[] filePaths)
    {

        modelPath = filePaths[0];
        RunPythonScript();
    }
    public async void RunPythonScript()
    {
        scriptPath = Path.Combine(Application.streamingAssetsPath, "classify.exe");
        jsonFilePath = SaveHandler.GetFilePath("_ModelForClassify");

        UnityEngine.Debug.Log(jsonFilePath);

        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(true);
            loadingCoroutine = StartCoroutine(LoadingAnimation());
        }

        string output = await RunPythonProcessAsync(scriptPath, $"\"{jsonFilePath}\" \"{modelPath}\"");
        StopLoadingAnimation();
        if (string.IsNullOrEmpty(output)) return;

        try
        {
            UnityEngine.Debug.Log($"Python Output: {output}");

            var match = System.Text.RegularExpressions.Regex.Match(output, @"Predicted class: (\d+) with confidence: (\d+\.\d+)%");

            if (match.Success)
            {
                int predictedClass = int.Parse(match.Groups[1].Value);
                float confidence = float.Parse(match.Groups[2].Value);

                string predictedClassName = predictedClass == 0 ? "Oak" : "Pine";

                string resultText = $"Predicted Class: <color=black><size=300%>{predictedClassName}<size=100%></color>\n" +
                                    $"<color=#8C8B8B>Confidence: {confidence}%</color>";

                // Обновляем UI
                pythonOutput.text = resultText;

                UnityEngine.Debug.Log(resultText);
                StartCoroutine(OutputFade());
            }
            else
            {
                UnityEngine.Debug.LogError("Unexpected output format.");
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError($"Error processing output: {e.Message}");
        }
    }


    private async Task<string> RunPythonProcessAsync(string fileName, string arguments)
    {
        return await Task.Run(() =>
        {
            try
            {
                ProcessStartInfo start = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = start })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        UnityEngine.Debug.LogError($"Greska u python skripti:\n{error}");
                    }

                    return output;
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log($"Greska u python skripti: {e.Message}");
                return string.Empty;
            }
        });
    }
    private IEnumerator LoadingAnimation()
    {
        string baseText = "Loading\n<color=black><size=300%>";
        int dotCount = 0;
        float startTime = Time.time; 

        while (true)
        {
            float elapsedTime = Time.time - startTime;

            string elapsedTimeString = $" {elapsedTime:F0} sec";

            loadingText.text = baseText + new string('.', dotCount) + "\n\n<color=white><size=50%>" + elapsedTimeString + "</color>";

            dotCount = (dotCount + 1) % 4;

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void StopLoadingAnimation()
    {
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }

        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(false);
        }
    }

    private IEnumerator OutputFade()
    {
        pythonOutput.color = new Color(pythonOutput.color.r, pythonOutput.color.g, pythonOutput.color.b, 1);
        yield return new WaitForSeconds(5f);
        while (pythonOutput.color.a > 0.01f)
        {
            yield return new WaitForFixedUpdate();

            pythonOutput.color = new Color(pythonOutput.color.r, pythonOutput.color.g, pythonOutput.color.b, Mathf.Lerp(pythonOutput.color.a, 0, 0.01f));

        }
        pythonOutput.color = new Color(pythonOutput.color.r, pythonOutput.color.g, pythonOutput.color.b, 0);
        lastPythonOutput.text = pythonOutput.text;
    }


}
