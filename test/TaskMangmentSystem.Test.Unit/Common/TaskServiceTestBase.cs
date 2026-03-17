using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Interfaces.Service;
using TaskManagmentSystem.API.Repositories.InMemory;
using TaskManagmentSystem.API.Services;

namespace TaskMangmentSystem.Test.Unit.Common
{
    public class TaskServiceTestBase
    {
        protected InMemoryUserRepo _userRepo = null!;
        protected ITaskService _taskService = null!;
        protected InMemoryTaskRepository _taskRepository = null!;

        [TestInitialize]
        public void TestInit()
        {
            var dbContext = new InMemoryDb();
            _userRepo = new InMemoryUserRepo(dbContext);
            _taskRepository = new InMemoryTaskRepository(dbContext);
            _taskService = new TaskService(_taskRepository);
        }
    }
}
