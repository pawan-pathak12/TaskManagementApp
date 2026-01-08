using TaskManagmentSystem.API.Enums;

namespace TaskManagmentSystem.API.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TasksStatus Status { get; set; } = TasksStatus.Pending;
    }
}
