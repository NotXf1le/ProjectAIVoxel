// SaveManager.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance => _instance;

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
        Instance.ClearSaveable();

    }

    public event Func<Task> OnBeforeSaveAsync;
    public event Func<Task> OnAfterSaveAsync;
    public event Func<Task> OnBeforeLoadAsync;
    public event Func<Task> OnAfterLoadAsync;
    public string LastSave { get; private set; }
    private readonly List<ISaveable> saveables = new List<ISaveable>();

    public void RegisterSaveable(ISaveable saveable) => saveables.Add(saveable);
    public void ClearSaveable() => saveables.Clear();

    public void SaveData(string slotName)
    {
   
        _ = SaveAsync(slotName);

    }
    public async Task SaveAsync(string slotName)
    {
        LastSave = slotName;

        Debug.Log("Saveoing");
        foreach (ISaveable savable in saveables)
        {
            Debug.Log(savable.ToString());


        }
        if (OnBeforeSaveAsync != null) await OnBeforeSaveAsync.Invoke();

        SaveData saveData = new SaveData();
        foreach (var saveable in saveables)
        {
            Debug.Log("Sav" + saveable.ToString());

            saveable.Save(saveData);
        }

        await SaveHandler.SaveToFileAsync(slotName, saveData);

        if (OnAfterSaveAsync != null) await OnAfterSaveAsync.Invoke();
    }

    public async Task LoadAsync(string slotName)
    {
        if (OnBeforeLoadAsync != null) await OnBeforeLoadAsync.Invoke();

        SaveData saveData = await SaveHandler.LoadFromFileAsync(slotName);
        foreach (var saveable in saveables)
        {
            saveable.Load(saveData);
        }

        if (OnAfterLoadAsync != null) await OnAfterLoadAsync.Invoke();
    }
    public async Task LoadPathAsync(string filePath)
    {
        if (OnBeforeLoadAsync != null) await OnBeforeLoadAsync.Invoke();

        SaveData saveData = await SaveHandler.LoadFromPath(filePath);
        foreach (var saveable in saveables)
        {
            saveable.Load(saveData);
        }

        if (OnAfterLoadAsync != null) await OnAfterLoadAsync.Invoke();
    }
}
