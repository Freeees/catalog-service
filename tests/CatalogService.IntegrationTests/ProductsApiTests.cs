using CatalogService.Application.Products;
using CatalogService.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace CatalogService.IntegrationTests;

[Collection("Database")]
public sealed class ProductsApiTests
{
    private const string TestProductName = "Bread";
    private const decimal TestProductPrice = 3.2m;

    private readonly HttpClient _client;

    public ProductsApiTests(PostgresFixture postgres)
    {
        var factory = new CustomWebAppFactory(postgres);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_then_Get_returns_created_product()
    {
        var create = new CreateProductRequest(TestProductName, TestProductPrice);

        var post = await _client.PostAsJsonAsync("/api/products", create);
        post.EnsureSuccessStatusCode();

        var created = await post.Content.ReadFromJsonAsync<ProductDto>();
        created.Should().NotBeNull();
        created!.Name.Should().Be(TestProductName);
        created!.Price.Should().Be(TestProductPrice);
        created!.CreatedAt.Should().BeCloseTo(
            DateTimeOffset.UtcNow,
            precision: TimeSpan.FromSeconds(5));

        var get = await _client.GetAsync($"/api/products/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
