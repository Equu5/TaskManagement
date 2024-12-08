public interface ITaskService
{
    Task<int> CreateTaskAsync(CreateTaskDto dto);
    Task UpdateTaskStatusAsync(int taskId, string newStatus);
    Task<TaskDto> GetTaskByIdAsync(int id);
}
