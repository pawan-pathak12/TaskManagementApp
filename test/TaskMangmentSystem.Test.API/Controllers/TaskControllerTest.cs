using System.Net;
using System.Net.Http.Json;
using TaskManagmentSystem.API.DTOs.Tasks;
using TaskManagmentSystem.API.Enums;
using TaskMangmentSystem.Test.API.Fixtures;

namespace TaskMangmentSystem.Test.API.Controllers
{
    [DoNotParallelize]
    [TestClass]
    public class TaskControllerTest : IntegrationTestBase
    {
        #region Positive Part 

        [TestMethod]
        public async Task CreateAsync_WhenValid_Return200()
        {
            //Arrange 
            var user = testDataBuilder.CreateAndReturnUser();

            var request = new CreateTaskDto
            {
                UserId = user.Id,
                Description = "Hello tesintg ",
                Title = "Testing"
            };

            //Act
            var response = await _client.PostAsJsonAsync("/api/Tasks", request);

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenTaskFound_Return200()
        {
            //Arrange 
            var user = await testDataBuilder.CreateAndReturnUser();
            var task = await testDataBuilder.CreateAndReturnTask(user.Id);

            //Act
            var response = await _client.GetAsync("/api/Tasks");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenTaskFound_Return200()
        {
            //Arrange 

            var user = await testDataBuilder.CreateAndReturnUser();
            var task = await testDataBuilder.CreateAndReturnTask(user.Id);

            //Act
            var response = await _client.GetAsync($"/api/Tasks/");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenTaskFoundAndValid_Return200()
        {
            //Arrange 

            var user = await testDataBuilder.CreateAndReturnUser();
            var task = await testDataBuilder.CreateAndReturnTask(user.Id);

            var update = new UpdateTaskDto
            {
                TaskId = task.Id,
                IsActive = task.IsActive,
                Title = "Updated Title",
                Description = "Updated description",
                Status = TasksStatus.InProgess,
                UpdateAt = DateTimeOffset.UtcNow
            };

            //Act
            var response = await _client.PutAsJsonAsync($"/api/Tasks/{update.TaskId}", update);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteAsync_WhenTaskSoftlyDeleted_Return200()
        {
            //Arrange 

            var user = await testDataBuilder.CreateAndReturnUser();
            var task = await testDataBuilder.CreateAndReturnTask(user.Id);

            //Act
            var response = await _client.DeleteAsync($"/api/Tasks/{task.Id}");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        #endregion

        #region Negative Part 

        [TestMethod]
        public async Task AnyRequest_WhenUserIsNotLogin_Return401()
        {
            //Arrange 

            //Act

            //Assert
        }

        [TestMethod]
        public async Task CreateAsync_WhenInValid_Return400()
        {
            //Arrange 
            var request = new CreateTaskDto
            {
                Description = "Hello tesintg ",
                Title = ""  // null title
            };

            //Act
            var response = await _client.PostAsJsonAsync("/api/Tasks", request);

            //Assert

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenNotFound_Return400()
        {
            var rand = new Random();
            var id = rand.Next(1000, 9999);
            //Act
            var response = await _client.GetAsync($"/api/Tasks/{id}");

            //Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenTaskNotFound_Return400()
        {
            //Arrange 
            var taskId = new Random().Next(0001, 9999);

            var update = new UpdateTaskDto
            {
                TaskId = taskId,
                Title = "Updated Title",
                Description = "Updated description",
                Status = TasksStatus.InProgess,
                UpdateAt = DateTimeOffset.UtcNow
            };

            //Act
            var response = await _client.PutAsJsonAsync($"/api/Tasks/{update.TaskId}", update);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestMethod]
        public async Task UpdateAsync_WhenInvalid_Return400()
        {
            //Arrange 

            var user = await testDataBuilder.CreateAndReturnUser();
            var task = await testDataBuilder.CreateAndReturnTask(user.Id);

            var update = new UpdateTaskDto
            {
                TaskId = task.Id,
                IsActive = task.IsActive,
                Title = "",   //null title 
                Description = "Updated description",
                Status = TasksStatus.InProgess,
                UpdateAt = DateTimeOffset.UtcNow
            };

            //Act
            var response = await _client.PutAsJsonAsync($"/api/Tasks/{update.TaskId}", update);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteAsync_WhenTaskNotFound_Return404()
        {
            //Arrange 
            var id = new Random().Next(001, 999);

            //Act
            var response = await _client.DeleteAsync($"/api/Tasks/{id}");

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}
