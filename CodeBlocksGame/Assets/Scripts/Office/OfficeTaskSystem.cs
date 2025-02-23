using UnityEngine;

public class OfficeTaskSystem : MonoBehaviour
{
    public static OfficeTaskSystem Instance;
    
    [SerializeField] private GameObject taskPanel;

    public OfficeWorker _selectedTaskHolder;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    public void SetTaskPanelVisual(bool isActive)
    {
        taskPanel.SetActive(isActive);
    }
    
    public void StartTask(OfficeWorker taskHolder)
    {
        _selectedTaskHolder = taskHolder;
        
        SetTaskPanelVisual(true);
    }
    
    public void ExitTask()
    {
        _selectedTaskHolder.ExitTask();
        
        SetTaskPanelVisual(false);
        
        _selectedTaskHolder = null;
    }
    
    public void CompleteTask()
    {
        _selectedTaskHolder.CompleteTask();
        
        SetTaskPanelVisual(false);
        
        _selectedTaskHolder = null;
    }
    
    public void FailTask()
    {
        _selectedTaskHolder.FailTask();
        
        SetTaskPanelVisual(false);
        
        _selectedTaskHolder = null;
    }
}
