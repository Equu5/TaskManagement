using Core.Domain;
using MediatR;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork, IMessagePublisher messagePublisher)
    {
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
    }

    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new Core.Domain.Entities.Task
        {
            Name = request.Name,
            Description = request.Description,
            AssignedTo = request.AssignedTo,
            Status = Core.Domain.Enums.TaskStatus.NotStarted
        };

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CommitAsync();

        var taskCreatedEvent = new TaskCreatedEvent(task.ID, task.Name);

        await _messagePublisher.PublishAsync(taskCreatedEvent, "task_exchange", "task.created");

        return task.ID;
    }
}
