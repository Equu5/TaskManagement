using Core.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITaskRepository Tasks { get; }
    Task CommitAsync();
}
