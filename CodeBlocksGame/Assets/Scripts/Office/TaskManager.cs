using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TaskManager : NetworkBehaviour
{
    public static TaskManager Instance;
    
    private List<int> tasks = new List<int>();
    
    private int _tasksMaxCount;
    
    private float _spawnTaskTimer;
    private float _spawnTaskTimerMax;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    private void Update()
    {
        if(!IsServer)
            return;
        
        _spawnTaskTimer -= Time.deltaTime;
        if (_spawnTaskTimer <= 0)
        {
            _spawnTaskTimer = _spawnTaskTimerMax;
            
            if(tasks.Count < _tasksMaxCount)
                SpawnNewTaskClientRpc();
        }
    }

    [ClientRpc]
    private void SpawnNewTaskClientRpc()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void CompleteTaskServerRpc(int taskId)
    {
        CompleteTaskClientRpc(taskId);
    }
    
    [ClientRpc]
    private void CompleteTaskClientRpc(int taskId)
    {
        
    }

    public void CompleteTaskTest()
    {
        ScoreSystem.Instance.AddScoreServerRpc(1);
    }
}
