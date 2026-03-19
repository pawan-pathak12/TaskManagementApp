using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagmentSystem.API.Data;

namespace TaskMangmentSystem.Test.Data.Fixtures
{
    public class DatabaseFixture : IDisposable
    {

        public IConfiguration? Configuration { get; }
        public ApplicationDbContext? DbContext { get; set; }

        public DatabaseFixture()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: false)
                .Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Configuration.GetConnectionString("LocalServer"))
                .Options;

            DbContext = new ApplicationDbContext(options);

        }
        public void Dispose()
        {
        }
    }
}
