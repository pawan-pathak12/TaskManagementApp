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
        Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash);
        #endregion

        #region Additional Common Operations

        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByUsernameAsync(string username);
        System.Threading.Tasks.Task UpdateLastLoginAsync(int userId, DateTime lastLogin, string? ipAddress);
        #endregion
    }
}