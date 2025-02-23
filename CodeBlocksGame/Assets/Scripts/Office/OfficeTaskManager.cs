using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class OfficeTaskManager : NetworkBehaviour
{
    public static OfficeTaskManager Instance;
    
    public event Action OnTaskCompleted;
    public event Action OnTaskFailed;
    
    [SerializeField] private List<OfficeWorker> _taskHolders;
    private Queue<int> _generateTaskRequestQueue;
    
    private int _tasksMaxCount = 1;
    private int _currentTaskCount = 0;
    
    private float _spawnTaskTimer;
    private float _spawnTaskTimerMax = 5f;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        
        _generateTaskRequestQueue = new Queue<int>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            foreach (var item in _taskHolders)
            {
                AddTaskInQueue(item);
            }
            _spawnTaskTimer = 5f;
        }
    }

    private void Update()
    {
        if(!IsServer)
            return;
        
        if(_generateTaskRequestQueue.Count <= 0 || _currentTaskCount >= _tasksMaxCount)
            return;
        
        _spawnTaskTimer -= Time.deltaTime;
        if (_spawnTaskTimer <= 0)
        {
            _spawnTaskTimer = _spawnTaskTimerMax;
            _currentTaskCount++;
            SpawnNewTaskClientRpc(_generateTaskRequestQueue.Dequeue());
        }
    }
    
    public void AddTaskInQueue(OfficeWorker taskHolder)
    {
        int index = _taskHolders.FindIndex(x => x == taskHolder);
        AddTaskInQueueServerRpc(index);
    }
    
    #region ServerRpc
    
    [ServerRpc(RequireOwnership = false)]
    public void CompleteTaskServerRpc()
    {
        _currentTaskCount--;
        CompleteTaskClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void FailTaskServerRpc()
    {
        _currentTaskCount--;
        FailTaskClientRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void AddTaskInQueueServerRpc(int index)
    {
        _generateTaskRequestQueue.Enqueue(index);
    }
    
    #endregion
    
    #region ClientRpc
    
    [ClientRpc]
    private void CompleteTaskClientRpc()
    {
        OnTaskCompleted?.Invoke();
        //ScoreSystem.Instance.AddScoreServerRpc(_completeTaskScore);
    }

    [ClientRpc]
    private void FailTaskClientRpc()
    {
        OnTaskFailed?.Invoke();
        //ScoreSystem.Instance.AddScoreServerRpc(_failTaskScore);
    }
    
    [ClientRpc]
    private void SpawnNewTaskClientRpc(int index)
    {
        OfficeWorker taskHolder = _taskHolders[index];
        
        Array array = Enum.GetValues(typeof(OfficeTaskType));
        OfficeTaskType taskType = (OfficeTaskType)array.GetValue(Random.Range(0, array.Length - 1));
        OfficeTask task = new OfficeTask(taskType,TimeSpan.FromSeconds(30));
        
        taskHolder.SetNewTask(task);
    }

    #endregion
}
