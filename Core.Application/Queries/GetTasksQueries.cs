using MediatR;

public class GetTasksQuery : IRequest<List<TaskDto>> { }
