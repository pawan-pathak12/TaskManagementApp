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

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenUserFound_ReturnTask()
        {
            //Arrange 

            //Act 

            //Assert

        }

        [TestMethod]
        public async Task GetAllAsync_WhenUserExists_ReturnTasks()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task UpdateAsync_WhenValidTask_ReturnTrue()
        {
            //Arrange 

            //Act 

            //Assert

        }

        [TestMethod]
        public async Task DeleteAsync_WhenTaskExists_ReturnTrue()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task UserExists_WhenUserExists_ReturnTrue()
        {
            //Arrange 

            //Act 

            //Assert
        }
        #endregion


        #region Negative Part 

        [TestMethod]
        public async Task CreateAsync_WhenTaskIsNull_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task CreateAsync_WhenTitleIsNull_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert
        }
        [TestMethod]
        public async Task CreateAsync_WhenTitleOutOfLimit_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert
        }


        [TestMethod]
        public async Task GetByIdAsync_WhenTaskNotFound_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert

        }


        [TestMethod]
        public async Task UpdateAsync_WhenTaskNotFound_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert

        }

        [TestMethod]
        public async Task DeleteAsync_WhenTaskNotFound_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task UserExists_WhenNotFound_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task UserExists_WhenUserIdIsNegative_ReturnFalse()
        {
            //Arrange 

            //Act 

            //Assert
        }

        #endregion
    }
}
