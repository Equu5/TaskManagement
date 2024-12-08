namespace Core.Domain.Exceptions;

public class TaskNotFoundException : Exception
{
    public TaskNotFoundException(int taskId)
        : base($"Task with ID {taskId} was not found.")
    {
    }
}
