using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Interfaces.Repositories;

namespace TaskManagmentSystem.API.Repositories.EFCore
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        #region CURD Operatons 

        public async Task<int> AddAsync(Entities.Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            if (user == null)
            {
                return false;
            }
            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<IEnumerable<Entities.Task>> GetAllAsync(int userId)
        {
            return await _context.Tasks
                                 .Where(x => x.UserId == userId)
                                 .ToListAsync();
        }


        public async Task<Entities.Task?> GetById(int id, int userId)
        {
            var user = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true && x.UserId == userId);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<bool> UpdateAsync(int id, Entities.Task task)
        {

            var existingTask = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingTask == null)
            {
                return false;
            }

            existingTask.IsActive = task.IsActive;
            existingTask.Description = task.Description;
            existingTask.Title = task.Title;
            task.UpdatedAt = DateTimeOffset.UtcNow;
            existingTask.Status = task.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region User 

        public async Task<bool> UserExists(int id)
        {
            return await _context.Users.AnyAsync(x => x.Id == id);

        }
        #endregion
    }
}
