using TaskManagmentSystem.API.Enums;

namespace TaskManagmentSystem.API.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TasksStatus Status { get; set; } = TasksStatus.Pending;
        public DateTimeOffset CreatedAt => DateTimeOffset.UtcNow;
        public bool IsActive { get; set; } = true;
    }

}
