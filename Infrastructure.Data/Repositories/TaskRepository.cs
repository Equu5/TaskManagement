using Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Core.Domain.Entities.Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            await _context.Tasks.AddAsync(task);
        }

        public async Task<Core.Domain.Entities.Task?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.ID == id);
        }

        public async Task<IEnumerable<Core.Domain.Entities.Task>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task UpdateAsync(Core.Domain.Entities.Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            var existingTask = await GetByIdAsync(task.ID);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {task.ID} not found.");
            }

            // Update fields
            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.AssignedTo = task.AssignedTo;

            _context.Tasks.Update(existingTask);
        }
    }
}
