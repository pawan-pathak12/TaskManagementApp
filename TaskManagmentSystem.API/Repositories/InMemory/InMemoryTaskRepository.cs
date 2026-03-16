using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Interfaces.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskManagmentSystem.API.Repositories.InMemory
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly InMemoryDb _db;
        private readonly List<Entities.TodoItem> _tasks;

        public InMemoryTaskRepository(InMemoryDb db)
        {
            _db = db;
            _tasks = _db.Tasks ?? new List<Entities.TodoItem>();
        }

        public Task<int> AddAsync(Entities.TodoItem task)
        {
            // Simple auto-increment ID
            task.Id = _tasks.Count > 0
                ? _tasks.Max(t => t.Id) + 1
                : 1;

            // Ensure required fields (optional - you can enforce in domain)
            task.CreatedAt = DateTime.UtcNow;
            if (task.Status == 0) task.Status = (Enums.TasksStatus)TaskStatus.Created; // default if not set

            _tasks.Add(task);
            return Task.FromResult(task.Id);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return Task.FromResult(false);
            }

            _tasks.Remove(task);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Entities.TodoItem>> GetAllAsync(int userId)
        {
            var userTasks = _tasks
                .Where(t => t.UserId == userId)
                .ToList();

            return Task.FromResult<IEnumerable<Entities.TodoItem>>(userTasks.AsReadOnly());
        }

        public Task<Entities.TodoItem?> GetById(int id, int userId)
        {
            var task = _tasks
                .FirstOrDefault(t => t.Id == id && t.UserId == userId);

            return Task.FromResult(task);
        }

        public Task<bool> UpdateAsync(int id, Entities.TodoItem task)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null)
            {
                return Task.FromResult(false);
            }

            // Only allow updating fields that make sense
            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.Status = task.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            // UserId should not be changeable in most cases
            // existing.UserId   = task.UserId;   ← usually not allowed

            return Task.FromResult(true);
        }

        public Task<bool> UserExists(int userId)
        {
            var exists = _tasks.Any(t => t.UserId == userId);
            return Task.FromResult(exists);
        }
    }
}