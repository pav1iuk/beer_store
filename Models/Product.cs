namespace BeerStore.Api.Models
{
    public class Product
    {
        public long ProductId { get; set; }            
        public string Sku { get; set; } = null!;       
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;

        public ProductDetail? Detail { get; set; }    
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
