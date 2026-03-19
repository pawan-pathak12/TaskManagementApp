namespace TaskMangmentSystem.Test.Data.Fixtures
{
    public abstract class IntegrationTestBase
    {
        protected readonly DatabaseFixture _databaseFixture = null!;
        protected IntegrationTestBase(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
        }

    }
}
