namespace BeerStore.Api.Models
{
    public class ProductImage
    {
        public long ImageId { get; set; }
        public long ProductId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsMain { get; set; } = false;
        public Product Product { get; set; } = null!;
    }

}
