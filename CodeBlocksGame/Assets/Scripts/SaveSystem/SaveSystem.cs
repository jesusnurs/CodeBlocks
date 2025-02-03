public class SaveSystem
{
	private SaveData mySaveData;
	private SaveData _saveData;
	private bool cached;
	
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

	public void UpdateSaveData()
	{
		SaveFileManager.UpdateSaveData(_saveData);
	}

	private void OnApplicationQuit()
	{
		UpdateSaveData();
	}
}