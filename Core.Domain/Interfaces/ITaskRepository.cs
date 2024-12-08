namespace Core.Domain.Interfaces;

public interface ITaskRepository : IAggregateRoot
{
    Task AddAsync(Entities.Task task);
    Task<Entities.Task?> GetByIdAsync(int id);
    Task<IEnumerable<Entities.Task>> GetAllAsync();
    Task UpdateAsync(Entities.Task task);
}