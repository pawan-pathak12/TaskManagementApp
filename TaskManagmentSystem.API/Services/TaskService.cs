using TaskManagmentSystem.API.Interfaces.Repositories;
using TaskManagmentSystem.API.Interfaces.Service;

namespace TaskManagmentSystem.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<int> CreateAsync(Entities.Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (task.UserId <= 0)
                throw new ArgumentException("UserId must be a positive number.", nameof(task.UserId));

            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title is required.", nameof(task.Title));

            if (task.Title.Length > 200)
                throw new ArgumentException("Task title cannot exceed 200 characters.", nameof(task.Title));

            // Optional: more business rules
            // if (task.DueDate.HasValue && task.DueDate < DateTime.UtcNow)
            //     throw new ArgumentException("Due date cannot be in the past.");

            return await _taskRepository.AddAsync(task);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Task ID must be positive.", nameof(id));

            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Entities.Task>> GetAllAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be positive.", nameof(userId));

            return await _taskRepository.GetAllAsync(userId);
        }

        public async Task<Entities.Task?> GetById(int id, int userId)
        {
            if (id <= 0)
                throw new ArgumentException("Task ID must be positive.", nameof(id));

            if (userId <= 0)
                throw new ArgumentException("UserId must be positive.", nameof(userId));

            return await _taskRepository.GetById(id, userId);
        }



        public async Task<bool> UpdateAsync(int id, Entities.Task task)
        {
            if (id <= 0)
                throw new ArgumentException("Task ID must be positive.", nameof(id));

            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title is required.", nameof(task.Title));

            if (task.Title.Length > 200)
                throw new ArgumentException("Task title cannot exceed 200 characters.", nameof(task.Title));

            // Optional: prevent changing UserId (security)
            // if (task.UserId != existingUserId) → fetch existing and compare

            return await _taskRepository.UpdateAsync(id, task);
        }

        public async Task<bool> UserExists(int id)
        {
            if (id <= 0)
                throw new ArgumentException("UserId must be positive.", nameof(id));

            return await _taskRepository.UserExists(id);
        }
    }
}