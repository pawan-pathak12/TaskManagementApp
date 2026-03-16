using TaskManagmentSystem.API.Entities;

namespace TaskManagmentSystem.API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        #region Core CRUD Operations

        Task<int> AddAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        #endregion

        #region Authentication & Lookup Methods

        Task<User?> GetByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);

        #endregion

        #region Additional Common Operations

        Task<IEnumerable<User>> GetAllAsync();


        #endregion
    }
}