namespace BeerStore.Api.Models
{
    public class Category
    {
        public long CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
