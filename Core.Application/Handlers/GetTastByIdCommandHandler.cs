using MediatR;
using AutoMapper;

public class GetTaskByIdCommandHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTaskByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        // Fetch the task by ID using the repository
        var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id);

        // If no task is found, return null or throw an exception
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
        }

        // Map the task entity to TaskDto
        var taskDto = _mapper.Map<TaskDto>(task);

        return taskDto;
    }
}
