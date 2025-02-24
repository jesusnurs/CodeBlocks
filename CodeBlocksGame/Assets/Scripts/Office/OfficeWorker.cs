using System;
using Unity.Netcode;
using UnityEngine;

public class OfficeWorker : NetworkBehaviour, IInteractable
{
    [SerializeField] private GameObject taskIconGameObject;
    [SerializeField] private GameObject taskSolvingGameObject;
    
    private event Action<bool> OnTaskChanged;
    private event Action<bool> OnTaskCompleted;
    
    private NetworkVariable<bool> _interactableNetworkVariable = new NetworkVariable<bool>(false);

    public OfficeTask _currentOfficeTask;
    
    private void Awake()
    {
        OnTaskChanged += SetTaskVisuals;
        OnTaskCompleted += ShowTaskCompletedAnim;

        _currentOfficeTask = null;
        _interactableNetworkVariable.Value = true;
    }
    
    public void Interact()
    {
        if(!_interactableNetworkVariable.Value)
            return;
        
        _interactableNetworkVariable.Value = false;
        
        TryStartTask();
    }
    
    public void UnInteract()
    {
        ExitTask();
    }
    
    private void TryStartTask()
    {
        if(!_interactableNetworkVariable.Value && _currentOfficeTask == null)
            return;

        _interactableNetworkVariable.Value = false;
        
        OfficeTaskSystem.Instance.StartTask(this);
    }

    public void ExitTask()
    {
        _interactableNetworkVariable.Value = true;
    }

    public void SetNewTask(OfficeTask newTask)
    {
        _currentOfficeTask = newTask;
        OnTaskChanged.Invoke(true);
        _interactableNetworkVariable.Value = true;
    }

    public void CompleteTask()
    {
        ClearTaskServerRpc(true);
        _interactableNetworkVariable.Value = true;
        OfficeTaskManager.Instance.CompleteTaskServerRpc();
        OfficeTaskManager.Instance.AddTaskInQueue(this);
        
        OfficeTaskSystem.Instance.SetTaskPanelVisual(false);
    }
    
    public void FailTask()
    {
        ClearTaskServerRpc(false);
        _interactableNetworkVariable.Value = true;
        OfficeTaskManager.Instance.FailTaskServerRpc();
        OfficeTaskManager.Instance.AddTaskInQueue(this);
        
        OfficeTaskSystem.Instance.SetTaskPanelVisual(false);
    }
    
    private void SetTaskVisuals(bool value)
    {
        taskIconGameObject.SetActive(value);
    }

    private void ShowTaskCompletedAnim(bool isSuccess)
    {
        
    }

    #region ServerRpc
    
    [ServerRpc(RequireOwnership = false)]
    public void ClearTaskServerRpc(bool isSuccess)
    {
        ClearTaskClientRpc(isSuccess);
    }

    #endregion
    
    #region ClientRpc
    
    [ClientRpc]
    private void ClearTaskClientRpc(bool isSuccess)
    {
        _currentOfficeTask = null;
        OnTaskChanged.Invoke(false);
        OnTaskCompleted.Invoke(isSuccess);
    }

    #endregion
}
