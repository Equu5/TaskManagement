using System.ComponentModel.DataAnnotations;
using Core.Domain;
using Core.Domain.Interfaces;
using Moq;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly Mock<IMessagePublisher> _messagePublisherMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _messagePublisherMock = new Mock<IMessagePublisher>();

        _unitOfWorkMock.Setup(u => u.Tasks).Returns(_taskRepositoryMock.Object);

        _handler = new CreateTaskCommandHandler(_unitOfWorkMock.Object, _messagePublisherMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateTask_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Name = "Test Task",
            Description = "Test Description",
            AssignedTo = "John Doe"
        };

        _unitOfWorkMock.Setup(u => u.Tasks.AddAsync(It.IsAny<Core.Domain.Entities.Task>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync()).Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.Tasks.AddAsync(It.IsAny<Core.Domain.Entities.Task>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        _messagePublisherMock.Verify(m => m.PublishAsync(It.IsAny<TaskCreatedEvent>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
