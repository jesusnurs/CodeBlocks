using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveFileManager
{
    private static readonly string localFileName = "SaveFile";
    private static string LocalFilePath => Application.persistentDataPath + "/" + localFileName;
    public static Action<SaveData> OnSaveFileChanged { get; set; }
    
    public static SaveData GetLocalSaveData()
    {
        try
        {
            if (!File.Exists(LocalFilePath))
            {
                return CreateNewSaveFile();
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(LocalFilePath, FileMode.Open))
            {
                SaveData data = formatter.Deserialize(stream) as SaveData;

                if (data == null)
                {
                    Debug.LogWarning("Save file is corrupted. Creating a new one.");
                    return CreateNewSaveFile();
                }

                data.FixNullReferences();
                return data;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load save data: {ex.Message}");
            return CreateNewSaveFile();
        }
    }
    
    public static void DeleteSaveFile()
    {
        try
        {
            if (File.Exists(LocalFilePath))
            {
                File.Delete(LocalFilePath);
                Debug.Log("Save file deleted successfully.");
            }
            else
            {
                Debug.LogWarning("No save file to delete.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to delete save file: {ex.Message}");
        }
    }
    
    public static void UpdateSaveData(SaveData newData)
    {
        try
        {
            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(LocalFilePath, FileMode.Create))
            {
                formatter.Serialize(stream, newData);
            }

            OnSaveFileChanged?.Invoke(newData);
            Debug.Log("Save data updated successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update save data: {ex.Message}");
        }
    }


    private static SaveData CreateNewSaveFile()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(LocalFilePath, FileMode.Create))
            {
                var saveData = SaveFileFactory.GetSaveDataInstance().CreateSaveData();
                formatter.Serialize(stream, saveData);

                OnSaveFileChanged?.Invoke(saveData);
                Debug.Log("New save file created successfully.");
                return saveData;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to create a new save file: {ex.Message}");
            return null;
        }
    }
}