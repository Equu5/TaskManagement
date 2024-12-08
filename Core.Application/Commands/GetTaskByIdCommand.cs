using MediatR;

public class GetTaskByIdCommand : IRequest<TaskDto>
{
    public int Id { get; set; }
}
