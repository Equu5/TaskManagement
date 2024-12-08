using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Interfaces;

public class UpdateTaskStatusCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;

    private readonly Mock<IMessagePublisher> _messagePublisherMock;
    private readonly UpdateTaskStatusCommandHandler _handler;

    public UpdateTaskStatusCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _messagePublisherMock = new Mock<IMessagePublisher>();

        _unitOfWorkMock.Setup(u => u.Tasks).Returns(_taskRepositoryMock.Object);

        _handler = new UpdateTaskStatusCommandHandler(_unitOfWorkMock.Object, _messagePublisherMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldPublishTaskUpdatedEvent_WhenStatusIsCompleted()
    {
        // Arrange
        var task = new Core.Domain.Entities.Task
        {
            ID = 1,
            Name = "Test Task",
            Description = "Test Description"
        };

        _unitOfWorkMock
            .Setup(u => u.Tasks.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(task);

        _unitOfWorkMock
            .Setup(u => u.Tasks.UpdateAsync(It.IsAny<Core.Domain.Entities.Task>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .Returns(Task.FromResult(1));

        var command = new UpdateTaskStatusCommand
        {
            TaskId = 1,
            NewStatus = "Completed"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.Tasks.UpdateAsync(It.Is<Core.Domain.Entities.Task>(t => t.ID == 1 && t.Status == Core.Domain.Enums.TaskStatus.Completed)), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);

        _messagePublisherMock.Verify(m => m.PublishAsync(
            It.Is<TaskStatusChangedEvent>(e => e.TaskId == 1 && e.NewStatus == "Completed"),
            "task_exchange",
            "task.status_changed"), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotPublishEvent_WhenStatusIsNotCompleted()
    {
        // Arrange
        var task = new Core.Domain.Entities.Task
        {
            ID = 1,
            Name = "Test Task",
            Description = "Test Description",
        };

        _unitOfWorkMock
            .Setup(u => u.Tasks.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(task);

        _unitOfWorkMock
            .Setup(u => u.Tasks.UpdateAsync(It.IsAny<Core.Domain.Entities.Task>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .Returns(Task.FromResult(1));

        var command = new UpdateTaskStatusCommand
        {
            TaskId = 1,
            NewStatus = "InProgress"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.Tasks.UpdateAsync(It.Is<Core.Domain.Entities.Task>(t => t.ID == 1 && t.Status == Core.Domain.Enums.TaskStatus.InProgress)), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);

        _messagePublisherMock.Verify(m => m.PublishAsync(
            It.IsAny<TaskUpdatedEvent>(),
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Never);
    }
}
