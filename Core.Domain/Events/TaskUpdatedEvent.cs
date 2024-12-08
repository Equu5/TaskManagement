namespace Core.Domain;

public class TaskUpdatedEvent : IEvent
{
    public int TaskId { get; }
    public string NewStatus { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TaskUpdatedEvent(int taskId, string newStatus)
    {
        TaskId = taskId;
        NewStatus = newStatus;
    }
}
