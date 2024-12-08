namespace Core.Domain.Entities;

public class Task
{
    public int ID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Enums.TaskStatus Status { get; set; }
    public string? AssignedTo { get; set; }
}