using AutoMapper;
using TaskManagmentSystem.API.DTOs.Tasks;

namespace TaskManagmentSystem.API.Mapping
{

    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Entities.Task, CreateTaskDto>().ReverseMap();
            CreateMap<Entities.Task, UpdateTaskDto>().ReverseMap();
            CreateMap<Entities.Task, ResponseTaskDto>().ReverseMap();
        }

    }
}
