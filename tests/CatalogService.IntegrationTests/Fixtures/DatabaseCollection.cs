namespace CatalogService.IntegrationTests.Fixtures;

[CollectionDefinition("Database")]
public sealed class DatabaseCollection : ICollectionFixture<PostgresFixture>
{
}
