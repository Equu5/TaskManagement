using Microsoft.AspNetCore.Mvc;
using MediatR;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskRequestModel model)
    {
        var command = new CreateTaskCommand
        {
            Name = model.Name,
            Description = model.Description,
            AssignedTo = model.AssignedTo
        };
        var taskId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTaskById), new { id = taskId }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var query = new GetTaskByIdQuery { Id = id };
        var task = await _mediator.Send(query);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var query = new GetTasksQuery();
        var tasks = await _mediator.Send(query);
        return Ok(tasks);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusRequestModel model)
    {
        var command = new UpdateTaskStatusCommand
        {
            TaskId = id,
            NewStatus = model.NewStatus
        };
        await _mediator.Send(command);
        return NoContent();
    }
}
