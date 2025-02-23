public interface ITaskHolder
{
    void SetNewTask(OfficeTask newTask);
    void TryStartTask();
    void CompleteTask();
    void ExitTask();
    void FailTask();
}
