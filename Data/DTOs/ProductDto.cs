namespace BeerStore.Api.Data.DTOs
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductDetailDto? Detail { get; set; }
        public IEnumerable<string>? Categories { get; set; }
        public IEnumerable<string>? Images { get; set; }
    }

}
