using MediatR;

public class UpdateTaskStatusCommand : IRequest
{
    public int TaskId { get; set; }
    public required string NewStatus { get; set; }
}
