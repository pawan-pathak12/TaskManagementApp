using TaskManagmentSystem.API.Enums;

namespace TaskManagmentSystem.API.DTOs.Tasks
{
    public class UpdateTaskStatus
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TasksStatus Status { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset UpdateAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
