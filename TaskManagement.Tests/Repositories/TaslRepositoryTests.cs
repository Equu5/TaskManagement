using FluentAssertions;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

public class TaskRepositoryTests
{
    private readonly DbContextOptions<TaskDbContext> _options;

    public TaskRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public async Task AddAsync_ShouldAddTaskToDatabase()
    {
        // Arrange
        using var context = new TaskDbContext(_options);
        var repository = new TaskRepository(context);
        var task = new Core.Domain.Entities.Task
        {
            Name = "Test Task",
            Description = "Test Description"
        };

        // Act
        await repository.AddAsync(task);
        await context.SaveChangesAsync();

        // Assert
        var savedTask = await context.Tasks.FirstOrDefaultAsync(t => t.Name == "Test Task");
        savedTask.Should().NotBeNull();
        savedTask?.Description.Should().Be("Test Description");
    }
}
