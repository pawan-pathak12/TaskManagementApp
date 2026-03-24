using System.ComponentModel.DataAnnotations;

namespace TaskManagmentSystem.API.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
