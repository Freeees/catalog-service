using CatalogService.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CatalogService.IntegrationTests;

public sealed class CustomWebAppFactory : WebApplicationFactory<Program>
{
    private readonly PostgresFixture _postgres;

    public CustomWebAppFactory(PostgresFixture postgres)
    {
        _postgres = postgres;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Postgres"] = _postgres.Db.GetConnectionString()
            });
        });
    }
}