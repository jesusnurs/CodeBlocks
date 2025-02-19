using Zenject;

public class SaveSystem : IInitializable
{
	public static SaveSystem Instance;
	
	private SaveData mySaveData;
	private SaveData _saveData;
	private bool cached;
	
	public void Initialize()
	{
		Instance = this;
	}
	
	public SaveData GetSaveData()
	{
		if (_saveData != null)
			return _saveData;

		if (cached == false)
		{
			mySaveData = SaveFileManager.GetLocalSaveData();
			_saveData = mySaveData;

			cached = true;
		}

		return mySaveData;
	}

	private void UpdateSaveData()
	{
		SaveFileManager.UpdateSaveData(_saveData);
	}

	private void OnApplicationQuit()
	{
		UpdateSaveData();
	}
}