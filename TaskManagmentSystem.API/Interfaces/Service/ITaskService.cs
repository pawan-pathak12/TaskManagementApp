using Task = TaskManagmentSystem.API.Entities.Task;
namespace TaskManagmentSystem.API.Interfaces.Service
{
    public interface ITaskService
    {
        Task<int> CreateAsync(Task task);
        Task<IEnumerable<Task>> GetAllAsync(int userId);
        Task<Task?> GetById(int id, int userId);
        Task<bool> UpdateAsync(int id, Task task);
        Task<bool> DeleteAsync(int id);

        #region User 

        Task<bool> UserExists(int id);
        #endregion
    }
}
