using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Repositories;

namespace TaskManagmentSystem.API.Repositories.EFCore
{
    public class UserRepository : IUserRepository
    {
        public Task<int> AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExistsAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
