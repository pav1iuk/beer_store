namespace BeerStore.Api.Models
{
    public class ProductDetail
    {
        public long DetailID { get; set; }
        public long ProductId { get; set; }            
        public string? Description { get; set; }
        public decimal? AlcoholPercent { get; set; }
        public int? VolumeMl { get; set; }
        public string? Country { get; set; }
        public Product Product { get; set; } = null!;
    }

}
