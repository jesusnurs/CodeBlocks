using System;
using Unity.Netcode;
using UnityEngine;

public class OfficeWorker : NetworkBehaviour, IInteractable
{
    [SerializeField] private GameObject taskIconGameObject;
    [SerializeField] private GameObject taskSolvingGameObject;
    
    private event Action<bool> OnTaskChanged;
    private event Action<bool> OnTaskCompleted;
    
    public bool _interactable;
    
    public OfficeTask _currentOfficeTask;
    
    private void Awake()
    {
        OnTaskChanged += SetTaskVisuals;
        OnTaskCompleted += ShowTaskCompletedAnim;

        _currentOfficeTask = null;
        _interactable = true;
    }
    
    public void Interact()
    {
        if(!_interactable)
            return;
        
        _interactable = false;
        
        TryStartTask();
    }
    
    public void UnInteract()
    {
        ExitTask();
    }
    
    private void TryStartTask()
    {
        if(!_interactable && _currentOfficeTask == null)
            return;
        
        SetInteractableServerRpc(false);
        
        OfficeTaskSystem.Instance.StartTask(this);
    }

    public void ExitTask()
    {
        SetInteractableServerRpc(true);
    }

    public void SetNewTask(OfficeTask newTask)
    {
        _currentOfficeTask = newTask;
        OnTaskChanged.Invoke(true);
        SetInteractableServerRpc(true);
    }

    public void CompleteTask()
    {
        ClearTaskServerRpc(true);
        SetInteractableServerRpc(true);
        OfficeTaskManager.Instance.CompleteTaskServerRpc();
        OfficeTaskManager.Instance.AddTaskInQueue(this);
        
        OfficeTaskSystem.Instance.SetTaskPanelVisual(false);
    }
    
    public void FailTask()
    {
        ClearTaskServerRpc(false);
        SetInteractableServerRpc(true);
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
    private void SetInteractableServerRpc(bool value)
    {
        SetInteractableClientRpc(value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClearTaskServerRpc(bool isSuccess)
    {
        ClearTaskClientRpc(isSuccess);
    }

    #endregion
    
    #region ClientRpc
    
    [ClientRpc]
    private void SetInteractableClientRpc(bool value)
    {
        _interactable = value;
    }
    
    [ClientRpc]
    private void ClearTaskClientRpc(bool isSuccess)
    {
        _currentOfficeTask = null;
        OnTaskChanged.Invoke(false);
        OnTaskCompleted.Invoke(isSuccess);
    }

    #endregion
}
