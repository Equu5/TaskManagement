using Core.Domain;

public class TaskEventHandler
{
    public async Task HandleTaskCreatedEvent(TaskCreatedEvent taskEvent)
    {
        Console.WriteLine($"Task Created: ID={taskEvent.TaskId}, Name={taskEvent.Name}");
        await Task.CompletedTask;
    }

    public async Task HandleTaskUpdatedEvent(TaskUpdatedEvent taskEvent)
    {
        Console.WriteLine($"Task Updated: ID={taskEvent.TaskId}, New Status={taskEvent.NewStatus}");
        await Task.CompletedTask;
    }
}
