using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Enums;
using TaskMangmentSystem.Test.Unit.Common;

namespace TaskMangmentSystem.Test.Unit.Services
{
    [TestClass]
    public class TaskServiceTest : TaskServiceTestBase
    {
        #region Positive Part 

        [TestMethod]
        public async Task CreateAsync_WhenValid_ReturnTrue()
        {
            //Arrange 
            var userId = await _userRepo.AddAsync(ReturnUser());
            var task = ReturnTask(userId);

            //Act 

            var result = await _taskService.CreateAsync(task);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenUserFound_ReturnTask()
        {
            //Arrange 

            var userId = await _userRepo.AddAsync(ReturnUser());
            var task = ReturnTask(userId);
            var taskId = await _taskRepository.AddAsync(task);

            //Act 
            var result = await _taskService.GetByIdAsync(taskId, userId);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(task.Description, result.Description);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenUserExists_ReturnTasks()
        {
            //Arrange 

            var userId = await _userRepo.AddAsync(ReturnUser());
            var task = ReturnTask(userId);
            await _taskRepository.AddAsync(task);

            //Act 
            var result = await _taskService.GetAllAsync(userId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotInstanceOfType<IEnumerable<Task>>(result);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenValidTask_ReturnTrue()
        {
            //Arrange 
            var userId = await _userRepo.AddAsync(ReturnUser());
            var task = ReturnTask(userId);
            var taskId = await _taskRepository.AddAsync(task);

            var taskData = await _taskRepository.GetByIdAsync(taskId, userId);
            var update = new TodoItem
            {
                UserId = taskData.UserId,
                Id = taskData.Id,
                IsActive = true,
                Status = TasksStatus.InProgess,
                Title = "Penmding "

            };

            //Act 
            var result = await _taskService.UpdateAsync(update.Id, update);

            //Assert
            Assert.IsTrue(result);

            var updatedTask = await _taskRepository.GetByIdAsync(taskId, userId);
            Assert.IsNotNull(updatedTask);
            Assert.AreEqual(update.Status, updatedTask.Status);
            Assert.AreEqual(update.Title, updatedTask.Title);
        }

        [TestMethod]
        public async Task DeleteAsync_WhenTaskExists_ReturnTrue()
        {
            //Arrange 

            var userId = await _userRepo.AddAsync(ReturnUser());
            var task = ReturnTask(userId);
            var taskId = await _taskRepository.AddAsync(task);

            //Act 
            var result = await _taskService.DeleteAsync(taskId, userId);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task UserExists_WhenUserExists_ReturnTrue()
        {
            //Arrange 
            var userId = await _userRepo.AddAsync(ReturnUser());

            //Act 
            var result = await _taskService.UserExists(userId);
            //Assert
            Assert.IsTrue(result);
        }
        #endregion


        #region Negative Part 

        [TestMethod]
        public async Task CreateAsync_WhenTaskIsNull_ReturnFalse()
        {
            //Arrange 
            var userId = await _userRepo.AddAsync(ReturnUser());
            var task = new TodoItem();

            //Act 
            var result = await _taskService.CreateAsync(task);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateAsync_WhenTitleIsNull_ReturnFalse()
        {
            //Arrange 
            var userId = await _userRepo.AddAsync(ReturnUser());
            var task = new TodoItem
            {
                UserId = userId,
                IsActive = true,
                Status = TasksStatus.Pending,
                Title = null,
                Description = "Something "
            };

            //Act 
            var result = await _taskService.CreateAsync(task);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateAsync_WhenTitleOutOfLimit_ReturnFalse()
        {
            //Arrange 
            var userId = await _userRepo.AddAsync(ReturnUser());
            var task = new TodoItem();

            //Act 
            var result = await _taskService.CreateAsync(task);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenTaskNotFound_ReturnNull()
        {
            //Arrange 

            var userId = await _userRepo.AddAsync(ReturnUser());
            var rand = new Random();
            var taskId = rand.Next(100, 999);

            //Act 

            var result = await _taskService.GetByIdAsync(taskId, userId);

            //Assert
            Assert.IsNull(result);

        }

        public async Task GetByIdAsync_WhenUserNotFound_ReturnNull()
        {
            //Arrange 
            var rand = new Random();
            var userId = rand.Next(100, 999);

            var taskId = await _taskRepository.AddAsync(ReturnTask(userId));
            //Act 

            var result = await _taskService.GetByIdAsync(taskId, userId);

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public async Task UpdateAsync_WhenTaskNotFound_ReturnFalse()
        {

            //Arrange 
            var userId = await _userRepo.AddAsync(ReturnUser());
            var rand = new Random();
            var taskId = rand.Next(100, 999);

            var update = new TodoItem
            {
                UserId = userId,
                Description = "ebg",
                IsActive = true,
                Id = taskId
            };

            //Act 

            var result = await _taskService.UpdateAsync(taskId, update);

            //Assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public async Task DeleteAsync_WhenTaskNotFound_ReturnFalse()
        {

            //Arrange 
            var userId = await _userRepo.AddAsync(ReturnUser());
            var rand = new Random();
            var taskId = rand.Next(100, 999);

            //Act 

            var result = await _taskService.DeleteAsync(taskId, userId);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UserExists_WhenNotFound_ReturnFalse()
        {

            //Arrange     
            var rand = new Random();
            var userId = rand.Next(100, 999);

            //Act 
            var result = await _taskService.UserExists(userId);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UserExists_WhenUserIdIsNegative_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert
        }

        #endregion


        #region Helper

        public User ReturnUser()
        {
            return new User
            {
                Email = "testing@gmail.com",
                IsActive = true,
                PasswordHash = "Hello WOrld",
                Role = "Admin"
            };
        }
        public TodoItem ReturnTask(int userId)
        {
            return new TodoItem
            {
                CreatedAt = DateTime.UtcNow,
                Description = "Hello World",
                IsActive = true,
                Title = "Unit Testing",
                Status = TasksStatus.Pending,
                UserId = userId
            };
        }
    }
        #endregion


}
