﻿public class TaskRequestModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? AssignedTo { get; set; }
}
