using Testcontainers.PostgreSql;

namespace CatalogService.IntegrationTests.Fixtures;

public sealed class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer Db { get; }

    public PostgresFixture()
    {
        Db = new PostgreSqlBuilder("postgres:16")
            .WithDatabase("catalogdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public Task InitializeAsync() => Db.StartAsync();

    public Task DisposeAsync() => Db.DisposeAsync().AsTask();
}
