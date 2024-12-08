namespace Core.Domain;

public class TaskCreatedEvent : IEvent
{
    public int TaskId { get; }
    public string Name { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TaskCreatedEvent(int taskId, string name)
    {
        TaskId = taskId;
        Name = name;
    }
}
