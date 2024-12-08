using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

public class TasksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TasksControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_CreateTask_ShouldReturnCreated()
    {
        // Arrange
        var task = new
        {
            Name = "New Task",
            Description = "Task Description",
            AssignedTo = "John Doe",
            Status = "NotStarted"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", task);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Get_Tasks_ShouldReturnOkWithTasks()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await response.Content.ReadFromJsonAsync<List<TaskDto>>();
        tasks.Should().NotBeEmpty();
    }
}
