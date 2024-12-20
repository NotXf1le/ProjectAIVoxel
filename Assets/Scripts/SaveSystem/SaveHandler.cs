using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveHandler
{
    public static async Task SaveToFileAsync(string slotName, SaveData data)
    {
        string json = JsonConvert.SerializeObject(data);
        Debug.Log($"Serialized JSON data: {json}");

        string filePath = GetFilePath(slotName);

        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            await writer.WriteAsync(json);
        }

        Debug.Log($"Data saved to: {filePath}");
        GenerateSaveIcon(slotName, Camera.main);
    }
    public static async Task<SaveData> LoadFromPath(string filePath)
    {

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Save file for {filePath} does not exist! Creating new save.");
            return new SaveData();
        }

        string json;
        using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
        {
            json = await reader.ReadToEndAsync();
        }

        Debug.Log($"Loaded JSON data: {json}");

        return JsonConvert.DeserializeObject<SaveData>(json);
    }
    public static async Task<SaveData> LoadFromFileAsync(string slotName)
    {
        string filePath = GetFilePath(slotName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Save file for {slotName} does not exist! Creating new save.");
            return new SaveData();
        }

        string json;
        using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
        {
            json = await reader.ReadToEndAsync();
        }

        Debug.Log($"Loaded JSON data: {json}");

        return JsonConvert.DeserializeObject<SaveData>(json);
    }

    public static string GetFilePath(string slotName)
    {
        return Path.Combine(Application.persistentDataPath, $"{slotName}.save");
    }
    private static void GenerateSaveIcon(string slotName, Camera camera)
    {
        if (camera == null)
        {
            Debug.LogError("Camera is null. Cannot generate save icon.");
            return;
        }

        RenderTexture renderTexture = new RenderTexture(256, 256, 24);
        camera.targetTexture = renderTexture;
        camera.Render();

        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null;
        Object.Destroy(renderTexture);

        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
        {
            string iconPath = Path.Combine(Application.persistentDataPath, $"/icons/{slotName}_icon.png");
            File.WriteAllBytes(iconPath, pngData);
            Debug.Log($"Icon saved to: {iconPath}");
        }
        else
        {
            Debug.LogError("Failed to encode texture to PNG.");
        }

        Object.Destroy(texture);
    }
}
