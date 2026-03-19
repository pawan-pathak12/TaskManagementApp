using System.Transactions;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Repositories;
using TaskManagmentSystem.API.Repositories;
using TaskMangmentSystem.Test.Data.Fixtures;

namespace TaskMangmentSystem.Test.Data.Repositories
{
    [DoNotParallelize]
    [TestClass]
    public class UserRepositoryTest
    {
        private IUserRepository _userRepository = null!;
        private TransactionScope _scope = null!;

        [TestInitialize]
        public void Init()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var db = new DatabaseFixture();
            _userRepository = new UserRepository(db.DbContext!);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _scope?.Dispose();
        }

        [TestMethod]
        public async Task AddAsync_WhenValid_ReturnUserId()
        {
            // Arrange
            var user = ReturnUser();

            // Act
            var userId = await _userRepository.AddAsync(user);

            // Assert
            Assert.IsGreaterThan(0, userId);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenUserExists_ReturnUser()
        {
            // Arrange
            var user = ReturnUser();
            var addedId = await _userRepository.AddAsync(user);

            // Act
            var retrievedUser = await _userRepository.GetByIdAsync(addedId);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(addedId, retrievedUser.Id);
            Assert.AreEqual(user.Email, retrievedUser.Email);
            Assert.AreEqual(user.Role, retrievedUser.Role);
            Assert.IsTrue(retrievedUser.IsActive);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenUserDoesNotExist_ReturnNull()
        {
            // Arrange
            int nonExistingId = 999999;

            // Act
            var retrievedUser = await _userRepository.GetByIdAsync(nonExistingId);

            // Assert
            Assert.IsNull(retrievedUser);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenUserExists_ReturnTrue()
        {
            // Arrange
            var user = ReturnUser();
            var userId = await _userRepository.AddAsync(user);

            var updatedUser = new User
            {
                Id = userId,
                Email = user.Email,
                Role = "SuperAdmin",
                PasswordHash = "NewHashedPass!",
                IsActive = true
            };

            // Act
            var result = await _userRepository.UpdateAsync(updatedUser);

            // Assert
            Assert.IsTrue(result);

            var refreshed = await _userRepository.GetByIdAsync(userId);
            Assert.AreEqual(updatedUser.Role, refreshed.Role);
            Assert.AreEqual(updatedUser.PasswordHash, refreshed.PasswordHash);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenUserDoesNotExist_ReturnFalse()
        {
            // Arrange
            var fakeUser = new User
            {
                Id = 999999,
                Email = "fake@example.com",
                Role = "User"
            };

            // Act
            var result = await _userRepository.UpdateAsync(fakeUser);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteAsync_WhenUserActive_ReturnSetsIsActiveFalseAndReturnTrue()
        {
            // Arrange
            var user = ReturnUser();
            var userId = await _userRepository.AddAsync(user);

            // Act
            var result = await _userRepository.DeleteAsync(userId);

            // Assert
            Assert.IsTrue(result);

            var deletedUser = await _userRepository.GetByIdAsync(userId);
            Assert.IsNotNull(deletedUser);
            Assert.IsFalse(deletedUser.IsActive);
        }

        [TestMethod]
        public async Task DeleteAsync_WhenUserDoesNotExist_ReturnFalse()
        {
            // Arrange
            int nonExistingId = 999999;

            // Act
            var result = await _userRepository.DeleteAsync(nonExistingId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetByEmailAsync_WhenUserExists_ReturnUser()
        {
            // Arrange
            var user = ReturnUser();
            await _userRepository.AddAsync(user);

            // Act
            var foundUser = await _userRepository.GetByEmailAsync(user.Email);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(user.Id, foundUser.Id);
            Assert.AreEqual(user.Email, foundUser.Email);
            Assert.IsTrue(foundUser.IsActive);
        }

        [TestMethod]
        public async Task GetByEmailAsync_WhenEmailDoesNotExist_ReturnNull()
        {
            // Arrange
            string nonExistingEmail = "doesnotexist@domain.com";

            // Act
            var foundUser = await _userRepository.GetByEmailAsync(nonExistingEmail);

            // Assert
            Assert.IsNull(foundUser);
        }

        [TestMethod]
        public async Task UserExistsAsync_WhenUserExists_ReturnTrue()
        {
            // Arrange
            var user = ReturnUser();
            await _userRepository.AddAsync(user);

            // Act
            var exists = await _userRepository.UserExistsAsync(user.Email);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task UserExistsAsync_WhenUserDoesNotExist_ReturnFalse()
        {
            // Arrange
            string nonExistingEmail = "noone@domain.com";

            // Act
            var exists = await _userRepository.UserExistsAsync(nonExistingEmail);

            // Assert
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenUserFound_ReturnUsers()
        {
            // Arrange
            var user1 = ReturnUser();
            var user2 = ReturnUser();

            await _userRepository.AddAsync(user1);
            await _userRepository.AddAsync(user2);

            // Act
            var users = await _userRepository.GetAllAsync();

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count());
            Assert.IsTrue(users.Any(u => u.Email == user1.Email));
            Assert.IsTrue(users.Any(u => u.Email == user2.Email));
        }

        [TestMethod]
        public async Task GetAllAsync_WhenNoActiveUsers_ReturnEmpty()
        {
            // Act
            var users = await _userRepository.GetAllAsync();

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(0, users.Count());
        }

        private User ReturnUser()
        {
            var rand = new Random();
            return new User
            {
                Email = $"testuser{rand.Next(10000, 99999)}@gmail.com",
                IsActive = true,
                Role = "Admin",
                PasswordHash = "HelloTesting!",
            };
        }
    }
}