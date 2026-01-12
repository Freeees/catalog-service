using CatalogService.Application.Products;
using CatalogService.Infrastructure.Persistence;
using CatalogService.Infrastructure.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("Postgres")
                 ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");

        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(cs));
        services.AddScoped<IProductRepository, EfProductRepository>();

        return services;
    }
}
