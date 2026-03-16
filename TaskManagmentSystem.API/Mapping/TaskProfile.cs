using AutoMapper;
using TaskManagmentSystem.API.DTOs.Tasks;

namespace TaskManagmentSystem.API.Mapping
{

    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Entities.TodoItem, CreateTaskDto>().ReverseMap();
            CreateMap<Entities.TodoItem, UpdateTaskDto>().ReverseMap();
            CreateMap<Entities.TodoItem, ResponseTaskDto>().ReverseMap();
        }

    }
}
