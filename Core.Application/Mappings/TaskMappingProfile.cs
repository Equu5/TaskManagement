using AutoMapper;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<Core.Domain.Entities.Task, TaskDto>();
        CreateMap<CreateTaskCommand, Core.Domain.Entities.Task> ();
    }
}
