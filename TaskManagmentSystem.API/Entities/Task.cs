using TaskManagmentSystem.API.Enums;

namespace TaskManagmentSystem.API.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TasksStatus Status { get; set; } = TasksStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
