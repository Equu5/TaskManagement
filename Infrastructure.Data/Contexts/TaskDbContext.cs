using Microsoft.EntityFrameworkCore;

public class TaskDbContext : DbContext
{
    public DbSet<Core.Domain.Entities.Task> Tasks { get; set; }

    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
    }
}
