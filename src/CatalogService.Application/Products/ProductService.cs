using CatalogService.Domain.Products;

namespace CatalogService.Application.Products;

public sealed class ProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<ProductDto?> GetAsync(Guid id, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        return p is null ? null : ToDto(p);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest req, CancellationToken ct)
    {
        var p = new Product(req.Name, req.Price);
        await _repo.AddAsync(p, ct);
        await _repo.SaveChangesAsync(ct);
        return ToDto(p);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProductRequest req, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        if (p is null) return false;

        p.Update(req.Name, req.Price);
        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        if (p is null) return false;

        await _repo.DeleteAsync(p, ct);
        await _repo.SaveChangesAsync(ct);
        return true;
    }

    private static ProductDto ToDto(Product p) => new(p.Id, p.Name, p.Price, p.CreatedAt);
}