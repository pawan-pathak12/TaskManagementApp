using TodoItem = TaskManagmentSystem.API.Entities.TodoItem;
namespace TaskManagmentSystem.API.Interfaces.Service
{
    public interface ITaskService
    {
        Task<int> CreateAsync(TodoItem task);
        Task<IEnumerable<TodoItem>> GetAllAsync(int userId);
        Task<TodoItem?> GetByIdAsync(int id, int userId);
        Task<bool> UpdateAsync(int id, TodoItem task);
        Task<bool> DeleteAsync(int id);

        #region User 

        Task<bool> UserExists(int id);
        #endregion
    }
}
