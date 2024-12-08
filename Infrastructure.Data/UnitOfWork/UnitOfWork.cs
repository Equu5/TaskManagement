using Core.Domain.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskDbContext _context;

    public ITaskRepository Tasks { get; }

    public UnitOfWork(TaskDbContext context, ITaskRepository taskRepository)
    {
        _context = context;
        Tasks = taskRepository;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}