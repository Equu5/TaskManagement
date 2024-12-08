using Core.Domain;

public class TaskStatusChangedEvent : IEvent
{
    public int TaskId { get; }
    public string NewStatus { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TaskStatusChangedEvent(int taskId, string newStatus)
    {
        TaskId = taskId;
        NewStatus = newStatus;
    }
}
