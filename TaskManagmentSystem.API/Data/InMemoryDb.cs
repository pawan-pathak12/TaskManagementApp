using TaskManagmentSystem.API.Entities;

namespace TaskManagmentSystem.API.Data
{
    public class InMemoryDb
    {
        public List<Entities.Task> Tasks { get; set; } = new();
        public List<User> User { get; set; } = new List<User>();
    }
}
