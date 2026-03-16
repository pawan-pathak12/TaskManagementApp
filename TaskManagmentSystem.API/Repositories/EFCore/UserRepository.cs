using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Repositories;

namespace TaskManagmentSystem.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Core CRUD Operations

        public async Task<int> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            // Soft delete example (preferred in most systems)
            user.IsActive = false;


            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Authentication & Lookup Methods

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash)
        {
            /* return await _context.Users
                 .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t =>
                     t.TokenHash == refreshTokenHash &&
                     !t.IsRevoked &&
                     t.ExpiresAt > DateTime.UtcNow));*/
            throw new NotImplementedException();
        }

        #endregion

        #region Additional Common Operations

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == username);
        }

        public async System.Threading.Tasks.Task UpdateLastLoginAsync(int userId, DateTime lastLogin, string? ipAddress)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                await _context.SaveChangesAsync();
            }
        }

        #endregion
    }
}