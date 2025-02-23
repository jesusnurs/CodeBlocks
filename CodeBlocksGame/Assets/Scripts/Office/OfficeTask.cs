using System;

public class OfficeTask
{
    public OfficeTaskType type;
    public TimeSpan ExpireTimeSpan;

    public OfficeTask() {}

    public OfficeTask(OfficeTaskType _type, TimeSpan _expireTimeSpan)
    {
        type = _type;
        ExpireTimeSpan = _expireTimeSpan;
    }

    public void StartTask()
    {
        //TasksSystem.Instance.StartTask(type);
    }
}

public enum OfficeTaskType
{
    One,
    Two,
    Three
}
