using MediatR;

public class GetTaskByIdQuery : IRequest<TaskDto>
{
    public int Id { get; set; }
}
