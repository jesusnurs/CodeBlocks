using UnityEngine;

[CreateAssetMenu(fileName = "SaveFileFactory", menuName = "SaveSystem/SaveFileFactory")]
public class SaveFileFactory : ScriptableObject
{
    [SerializeField]
    private SaveData startingSaveData;
        
    [SerializeField]
    private SaveData developerSaveData;
        
    [SerializeField] [Tooltip("true - developer save data\nfalse - user save data")]
    private bool saveDataVersion;

    public SaveData StartingSaveData => startingSaveData;
        
    public SaveData CreateSaveData()
    {
        return saveDataVersion
            ? developerSaveData
            : startingSaveData;
    }
        
    public static SaveFileFactory GetSaveDataInstance()
    {
        return Resources.Load<SaveFileFactory>("SaveFileFactory");
    }
}
