using TaskManagmentSystem.API.Interfaces.Repositories;
using TaskManagmentSystem.API.Interfaces.Service;
using TodoItem = TaskManagmentSystem.API.Entities.TodoItem;

namespace TaskManagmentSystem.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<bool> CreateAsync(TodoItem task)
        {
            // Basic validation - task object cannot be null
            if (task == null)
            {
                return false;
            }

            // UserId must be positive (0 or negative IDs are invalid)
            if (task.UserId <= 0)
            {
                return false;
            }

            // Title is required - cannot be null, empty or only whitespace
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                return false;
            }

            // Reasonable length limit - prevents abuse or database issues
            if (task.Title.Length > 200)
            {
                return false;
            }

            await _taskRepository.AddAsync(task);

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            if (id <= 0)
            {
                return false;
            }

            var task = await _taskRepository.GetById(id, userId);
            if (task == null)
            {
                return false;
            }
            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(int userId)
        {
            // UserId must be positive to avoid invalid queries
            if (userId <= 0)
                throw new ArgumentException("UserId must be positive.", nameof(userId));

            return await _taskRepository.GetAllAsync(userId);
        }

        public async Task<TodoItem?> GetByIdAsync(int id, int userId)
        {
            // Task ID must be positive
            if (id <= 0)
                throw new ArgumentException("Task ID must be positive.", nameof(id));

            // UserId must be positive - ownership check depends on it
            if (userId <= 0)
                throw new ArgumentException("UserId must be positive.", nameof(userId));

            return await _taskRepository.GetById(id, userId);
        }

        public async Task<bool> UpdateAsync(int id, TodoItem task)
        {
            // ID must be positive - cannot update non-existent/invalid task
            if (id <= 0)
            {
                return false;
            }
            // Task object cannot be null
            if (task == null)
            {
                return false;
            }

            // Title is required - same rule as create
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                return false;
            }

            // Same length limit as create - consistency
            if (task.Title.Length > 200)
            {
                return false;
            }


            return await _taskRepository.UpdateAsync(id, task);
        }

        public async Task<bool> UserExists(int id)
        {
            // UserId must be positive - invalid input otherwise
            if (id <= 0)
            {
                return false;
            }

            return await _taskRepository.UserExists(id);
        }
    }
}