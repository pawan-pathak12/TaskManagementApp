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
    public class IntegrationTestBase
    {
        protected static CustomWebApplicationFactory Factory = null!;
        protected HttpClient _client = null!;
        protected TestDataBuilder testDataBuilder = null!;

        protected IServiceProvider Services = null!;
        protected static Respawner _respawn = null!;
        protected static string _conectionString = null!;


        #region Class Init and Cleanup
        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static async Task ClassInit(TestContext context)
        {
            Factory = new CustomWebApplicationFactory();

            //get connection string from the factory dbcontext 

            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _conectionString = dbContext.Database.GetConnectionString()!;

            // ensure db is created 

            await dbContext.Database.EnsureCreatedAsync();

            // initilize Respawner

            using var connection = new SqlConnection(_conectionString);
            await connection.OpenAsync();

            _respawn = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[]
                {
                    "_EFMigrationsHistory"
                },
                DbAdapter = DbAdapter.SqlServer
            });
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]

        public static void ClassCleanUp()
        {
            Factory.Dispose();
        }
        #endregion

        #region Class Init and CleanUp

        [TestInitialize]
        public async Task TestInt()
        {
            //Reset db are every test 
            using var connection = new SqlConnection(_conectionString);
            await connection.OpenAsync();
            await _respawn.ResetAsync(connection);

            _client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                HandleCookies = true,
                AllowAutoRedirect = false
            });

            var scope = Factory.Services.CreateScope();
            Services = scope.ServiceProvider;
            testDataBuilder = new TestDataBuilder(Services);

            var user = await testDataBuilder.CreateAndReturnUser();
            var token = testDataBuilder.GenerateAndReturnToken(user!);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _client.Dispose();
        }

        public void SetRefreshTokenCookie(HttpResponseMessage loginResponse)
        {
            var cookies = loginResponse.Headers.GetValues("Set-Cookie")
                .Where(c => c.Contains("refreshToken="))
                .Select(c => c.Split(';')[0])
                .ToList();

            foreach (var cookie in cookies)
            {
                // rmeove existing cookie 
                if (_client.DefaultRequestHeaders.Contains("Cookie"))
                {
                    _client.DefaultRequestHeaders.Remove("Cookie");
                }
                _client.DefaultRequestHeaders.Add("Cookie", cookie);
            }

        }
        #endregion
    }
}
