using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.API.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required]
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
