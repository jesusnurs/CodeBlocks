using System;
using Unity.Netcode;

public class OfficeLevelManager : NetworkBehaviour
{
    public static OfficeLevelManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    public void GetCurrentLevelData()
    {
        
    }
}

public struct LevelData
{
    public int LevelId;
}
