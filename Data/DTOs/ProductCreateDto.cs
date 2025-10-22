namespace BeerStore.Api.Data.DTOs
{
    public class ProductCreateDto
    {
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public string? Description { get; set; }
        public decimal? AlcoholPercent { get; set; }
        public int? VolumeMl { get; set; }
        public string? Country { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsMainImage { get; set; } = true;

        public List<long>? CategoryIds { get; set; }
    }
}
