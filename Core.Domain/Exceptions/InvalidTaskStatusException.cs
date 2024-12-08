namespace Core.Domain.Exceptions;

public class InvalidTaskStatusException : Exception
{
    public InvalidTaskStatusException(TaskStatus status)
        : base($"Task status '{status}' is invalid.")
    {
    }
}
