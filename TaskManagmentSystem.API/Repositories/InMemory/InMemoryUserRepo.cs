using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Repositories;

namespace TaskManagmentSystem.API.Repositories.InMemory
{
    public class InMemoryUserRepo : IUserRepository
    {
        private readonly InMemoryDb _dbContext;
        private readonly List<User> _users;

        public InMemoryUserRepo(InMemoryDb dbContext)
        {
            _dbContext = dbContext;
            _users = _dbContext.User ?? new List<User>();
        }

        public Task<int> AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Id = _users.Count > 0
                ? _users.Max(u => u.Id) + 1
                : 1;

            user.IsActive = true;

            _users.Add(user);
            return Task.FromResult(user.Id);
        }

        public Task<User?> GetByIdAsync(int id)
        {
            var user = _users
                .FirstOrDefault(u => u.Id == id && u.IsActive);

            return Task.FromResult(user);
        }

        public Task<bool> UpdateAsync(User user)
        {
            var existing = _users
                .FirstOrDefault(u => u.Id == user.Id && u.IsActive);

            if (existing == null)
                return Task.FromResult(false);

            // Update allowed fields (prevent changing Id, IsActive, etc.)
            existing.Email = user.Email;
            existing.PasswordHash = user.PasswordHash;
            existing.Role = user.Role;
            existing.IsActive = user.IsActive;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id && u.IsActive);
            if (user == null)
                return Task.FromResult(false);

            user.IsActive = false;

            return Task.FromResult(true);
        }
        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users
                .FirstOrDefault(u =>
                    string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase) &&
                    u.IsActive);

            return Task.FromResult(user);
        }
        public Task<bool> UserExistsAsync(string email)
        {
            var exists = _users
                .Any(u =>
                    string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase) &&
                    u.IsActive);

            return Task.FromResult(exists);
        }


        public Task<IEnumerable<User>> GetAllAsync()
        {
            var activeUsers = _users
                .Where(u => u.IsActive)
                .ToList();

            return Task.FromResult<IEnumerable<User>>(activeUsers.AsReadOnly());
        }


    }
}