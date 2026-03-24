using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Net.Http.Headers;
using TaskManagmentSystem.API.Data;
using TaskMangmentSystem.Test.API.DataBuilder;

namespace TaskMangmentSystem.Test.API.Fixtures
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        protected static CustomWebApplicationFactory Factory = null!;
        protected HttpClient _client = null!;
        protected TestDataBuilder testDataBuilder = null!;
        protected IServiceProvider Services = null!;
        protected static Respawner _respawner = null!;
        protected static string _connectionString = null!;

        #region Class Initialize / Cleanup

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static async Task ClassInit(TestContext context)
        {
            Factory = new CustomWebApplicationFactory();

            // Get connection string once from real DbContext
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _connectionString = dbContext.Database.GetConnectionString()!;

            // Ensure database is created (only once per class init)
            await dbContext.Database.EnsureCreatedAsync();

            // Initialize Respawner (only once)
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { new("_EFMigrationsHistory") },
                DbAdapter = DbAdapter.SqlServer
            });
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassCleanUp()
        {
            Factory?.Dispose();
        }

        #endregion

        #region Test Initialize / Cleanup

        [TestInitialize]
        public async Task TestInit()
        {
            // Reset database state before every test (using Respawn)
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await _respawner.ResetAsync(connection);


            _client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                HandleCookies = true,
                AllowAutoRedirect = false
            });

            // Create scoped services
            var scope = Factory.Services.CreateScope();
            Services = scope.ServiceProvider;

            testDataBuilder = new TestDataBuilder(Services);

            // Create default authenticated user for most tests
            var user = await testDataBuilder.CreateAndReturnUser();
            if (user == null)
                throw new InvalidOperationException("Failed to create test user");

            var token = testDataBuilder.GenerateAndReturnToken(user);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _client?.Dispose();
        }

        #endregion

        #region Helpers

        protected void SetRefreshTokenCookie(HttpResponseMessage loginResponse)
        {
            if (loginResponse.Headers.TryGetValues("Set-Cookie", out var cookieValues))
            {
                var refreshCookies = cookieValues
                    .Where(c => c.Contains("refreshToken="))
                    .Select(c => c.Split(';')[0])
                    .ToList();

                foreach (var cookie in refreshCookies)
                {
                    // Remove any existing Cookie header first to avoid duplicates
                    if (_client.DefaultRequestHeaders.Contains("Cookie"))
                    {
                        _client.DefaultRequestHeaders.Remove("Cookie");
                    }

                    _client.DefaultRequestHeaders.Add("Cookie", cookie);
                }
            }
        }

        #endregion
    }
}