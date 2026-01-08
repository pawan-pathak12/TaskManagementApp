using TaskManagmentSystem.API.Enums;

namespace TaskManagmentSystem.API.DTOs.Tasks
{
    public class ResposneTaskDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TasksStatus Status { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool IsActive { get; set; }


    }
}
