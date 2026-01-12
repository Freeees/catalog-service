using CatalogService.Application.Products;
using CatalogService.Domain.Products;
using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Products;

public sealed class EfProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public EfProductRepository(AppDbContext db) => _db = db;

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct) =>
        await _db.Products
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);

    public Task AddAsync(Product product, CancellationToken ct) =>
        _db.Products.AddAsync(product, ct).AsTask();

    public Task DeleteAsync(Product product, CancellationToken ct)
    {
        _db.Products.Remove(product);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
