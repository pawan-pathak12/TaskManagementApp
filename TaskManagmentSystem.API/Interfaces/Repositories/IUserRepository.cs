using TaskManagmentSystem.API.Entities;

namespace TaskManagmentSystem.API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
    }
}
