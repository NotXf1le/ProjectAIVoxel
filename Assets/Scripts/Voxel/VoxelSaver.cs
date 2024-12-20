using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;

public class VoxelSaver : MonoBehaviour, ISaveable
{
    [SerializeField] private TMPro.TMP_InputField inputFieldSave;
    [SerializeField] private TMPro.TMP_Text classText;
    [SerializeField] private TMPro.TMP_Text loadedModelText;

	public string lastSavedModelName;
    private string voxelModelClass = "Oak";
    private void Start()
    {
        SaveManager.Instance.RegisterSaveable(this);
        classText.text = voxelModelClass;

	}
    public void Load(SaveData data)
    {
        voxelModelClass = data.GetData("VoxelClass", voxelModelClass);

        BlockPlacer.Instance.voxels = data.GetData("Voxels", BlockPlacer.Instance.voxels);
        List<GameObject> keys = new List<GameObject>(BlockPlacer.Instance.blocks.Keys);
        foreach (GameObject block in keys)
        {
            Destroy(block);
        }
        BlockPlacer.Instance.blocks.Clear();
        BlockPlacer.Instance.GenerateBlocks();

    }


    public void Save(SaveData data)
    {
        data.SetData("VoxelClass", voxelModelClass);
        data.SetData("Voxels", BlockPlacer.Instance.voxels);


    }
    public void MultiSave()
    {
        loadedModelText.text = "";

        StartCoroutine(MultipleSaveWithDelay());
    }
    private IEnumerator MultipleSaveWithDelay()
    {
        GameStateManager.Instance.ChangeState("Game");

        for (int i = 0; i < 8; i++)
        {
            BlockPlacer.Instance.Rotate90();
            SaveManager.Instance.SaveData(inputFieldSave.text.Trim().ToLower() + i + voxelModelClass.ToString());
			lastSavedModelName = inputFieldSave.text.Trim().ToLower() + i + voxelModelClass.ToString();

			yield return new WaitForSeconds(0.5f);

            if (i == 3)
                BlockPlacer.Instance.Flip("X");
        }

    }
    public void ChangeCurrentClass(string className)
    {
        voxelModelClass = className;
        classText.text = voxelModelClass;
    }
    public void SaveInputFieldValue()
    {
        loadedModelText.text = "";

        if (inputFieldSave != null)
        {
            SaveManager.Instance.SaveData(inputFieldSave.text.Trim().ToLower()+voxelModelClass.ToString());
			lastSavedModelName = inputFieldSave.text.Trim().ToLower() + voxelModelClass.ToString();

			GameStateManager.Instance.ChangeState("Game");
        }
    }

    public void LoadFile()
    {
		GameStateManager.Instance.ChangeState("LoadState");
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Models", ".save"));

        FileBrowser.SetDefaultFilter(".save");

        FileBrowser.AddQuickLink("Models", Application.persistentDataPath.ToString(), null);
        StartCoroutine(ShowLoadDialogCoroutine());

    }


	IEnumerator ShowLoadDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, Application.persistentDataPath.ToString(), null, "Select File", "Load");

		Debug.Log(FileBrowser.Success);

		if (FileBrowser.Success)
			OnFilesSelected(FileBrowser.Result); 
		GameStateManager.Instance.ChangeState("Game");

	}

	void OnFilesSelected(string[] filePaths)
	{

        string filePath = filePaths[0];
        loadedModelText.text = filePath;

        _ = SaveManager.Instance.LoadPathAsync(filePath);

	}
}
