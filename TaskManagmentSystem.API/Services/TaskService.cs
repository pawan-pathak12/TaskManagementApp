using TaskManagmentSystem.API.Interfaces.Repositories;
using TaskManagmentSystem.API.Interfaces.Service;
using Task = TaskManagmentSystem.API.Entities.Task;

namespace TaskManagmentSystem.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<int> CreateAsync(Task task)
        {
            // Basic validation - task object cannot be null
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            // UserId must be positive (0 or negative IDs are invalid)
            if (task.UserId <= 0)
                throw new ArgumentException("UserId must be a positive number.", nameof(task.UserId));

            // Title is required - cannot be null, empty or only whitespace
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title is required.", nameof(task.Title));

            // Reasonable length limit - prevents abuse or database issues
            if (task.Title.Length > 200)
                throw new ArgumentException("Task title cannot exceed 200 characters.", nameof(task.Title));

            // Optional: Due date should not be in the past (business rule)
            // if (task.DueDate.HasValue && task.DueDate.Value.Date < DateTime.UtcNow.Date)
            //     throw new ArgumentException("Due date cannot be in the past.", nameof(task.DueDate));

            return await _taskRepository.AddAsync(task);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // ID must be positive - 0 or negative doesn't make sense
            if (id <= 0)
                throw new ArgumentException("Task ID must be positive.", nameof(id));

            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Task>> GetAllAsync(int userId)
        {
            // UserId must be positive to avoid invalid queries
            if (userId <= 0)
                throw new ArgumentException("UserId must be positive.", nameof(userId));

            return await _taskRepository.GetAllAsync(userId);
        }

        public async Task<Task?> GetById(int id, int userId)
        {
            // Task ID must be positive
            if (id <= 0)
                throw new ArgumentException("Task ID must be positive.", nameof(id));

            // UserId must be positive - ownership check depends on it
            if (userId <= 0)
                throw new ArgumentException("UserId must be positive.", nameof(userId));

            return await _taskRepository.GetById(id, userId);
        }

        public async Task<bool> UpdateAsync(int id, Task task)
        {
            // ID must be positive - cannot update non-existent/invalid task
            if (id <= 0)
                throw new ArgumentException("Task ID must be positive.", nameof(id));

            // Task object cannot be null
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            // Title is required - same rule as create
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title is required.", nameof(task.Title));

            // Same length limit as create - consistency
            if (task.Title.Length > 200)
                throw new ArgumentException("Task title cannot exceed 200 characters.", nameof(task.Title));

            // Optional: prevent changing ownership (security)
            // You could fetch existing task and compare UserId
            // var existing = await _taskRepository.GetById(id, task.UserId);
            // if (existing == null || existing.UserId != task.UserId)
            //     throw new UnauthorizedAccessException("Cannot change task ownership.");

            return await _taskRepository.UpdateAsync(id, task);
        }

        public async Task<bool> UserExists(int id)
        {
            // UserId must be positive - invalid input otherwise
            if (id <= 0)
                throw new ArgumentException("UserId must be positive.", nameof(id));

            return await _taskRepository.UserExists(id);
        }
    }
}