using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TaskConfiguration : IEntityTypeConfiguration<Core.Domain.Entities.Task>
{
    public void Configure(EntityTypeBuilder<Core.Domain.Entities.Task> builder)
    {
        builder.ToTable("Tasks");
        builder.HasKey(t => t.ID);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.Status).IsRequired();
    }
}
