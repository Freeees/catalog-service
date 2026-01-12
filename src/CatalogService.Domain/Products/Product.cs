namespace CatalogService.Domain.Products
{
    public sealed class Product
    {
        private const int NameMaxLength = 200;
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = null!;
        public decimal Price { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        private Product() { } // for EF

        public Product(string name, decimal price)
        {
            SetName(name);
            SetPrice(price);
        }

        public void Update(string name, decimal price)
        {
            SetName(name);
            SetPrice(price);
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (name.Length > NameMaxLength)
                throw new ArgumentException("Name is too long.", nameof(name));

            Name = name.Trim();
        }

        private void SetPrice(decimal price)
        {
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price), "Price must be >= 0.");
            Price = price;
        }
    }
}
