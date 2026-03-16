using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Interfaces.Service;
using TaskManagmentSystem.API.Repositories.InMemory;
using TaskManagmentSystem.API.Services;

namespace TaskMangmentSystem.Test.Unit.Common
{
    public class TaskServiceTestBase
    {
        private ITaskService _taskService = null!;
        private InMemoryTaskRepository _taskRepository = null!;
        [TestInitialize]
        public void TestInit()
        {
            var dbContext = new InMemoryDb();
            _taskRepository = new InMemoryTaskRepository(dbContext);
            _taskService = new TaskService(_taskRepository);
        }
    }
}
