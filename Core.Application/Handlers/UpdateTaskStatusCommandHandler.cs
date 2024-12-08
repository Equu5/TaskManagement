using Core.Domain;
using MediatR;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;

    public UpdateTaskStatusCommandHandler(IUnitOfWork unitOfWork, IMessagePublisher messagePublisher)
    {
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);

        if (task == null)
            throw new KeyNotFoundException("Task not found.");

        if (!Enum.TryParse<Core.Domain.Enums.TaskStatus>(request.NewStatus, out var status))
            throw new ArgumentException("Invalid status value.");

        task.Status = status;
        await _unitOfWork.Tasks.UpdateAsync(task);
        await _unitOfWork.CommitAsync();

        var taskCreatedEvent = new TaskStatusChangedEvent(task.ID, request.NewStatus);

        await _messagePublisher.PublishAsync(taskCreatedEvent, "task_exchange", "task.status_changed");
    }
}