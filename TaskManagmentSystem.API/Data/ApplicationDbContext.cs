
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.API.Entities;

namespace TaskManagmentSystem.API.Data

{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Entities.TodoItem> Tasks => Set<Entities.TodoItem>();
    }
}
