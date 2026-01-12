using CatalogService.Application.Products;
using CatalogService.Domain.Products;
using FluentAssertions;

namespace CatalogService.UnitTests;

public sealed class ProductServiceTests
{
    private const string TestProductName = "Milk";
    private const decimal TestProductPrice = 2.5m;
    private sealed class FakeRepo : IProductRepository
    {
        public readonly List<Product> Items = new();
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
            Task.FromResult(Items.FirstOrDefault(x => x.Id == id));

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct) =>
            Task.FromResult((IReadOnlyList<Product>)Items.ToList());

        public Task AddAsync(Product product, CancellationToken ct) { Items.Add(product); return Task.CompletedTask; }
        public Task DeleteAsync(Product product, CancellationToken ct) { Items.Remove(product); return Task.CompletedTask; }
        public Task SaveChangesAsync(CancellationToken ct) => Task.CompletedTask;
    }

    [Fact]
    public async Task CreateAsync_creates_product()
    {
        var repo = new FakeRepo();
        var svc = new ProductService(repo);

        repo.Items.Should().HaveCount(0);

        var dto = await svc.CreateAsync(new CreateProductRequest(TestProductName, TestProductPrice), CancellationToken.None);

        dto.Name.Should().Be(TestProductName);
        dto.Price.Should().Be(TestProductPrice);
        dto.CreatedAt.Should().BeCloseTo(
            DateTimeOffset.UtcNow,
            precision: TimeSpan.FromSeconds(5));

        repo.Items.Should().HaveCount(1);
    }
}