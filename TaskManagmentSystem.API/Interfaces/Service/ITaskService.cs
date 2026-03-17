using TodoItem = TaskManagmentSystem.API.Entities.TodoItem;
namespace TaskManagmentSystem.API.Interfaces.Service
{
    public interface ITaskService
    {
        Task<bool> CreateAsync(TodoItem task);
        Task<IEnumerable<TodoItem>> GetAllAsync(int userId);
        Task<TodoItem?> GetByIdAsync(int taskId, int userId);
        Task<bool> UpdateAsync(int id, TodoItem task);
        Task<bool> DeleteAsync(int taskId, int userId);

        #region User 

        Task<bool> UserExists(int id);
        #endregion
    }
}
