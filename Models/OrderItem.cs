namespace BeerStore.Api.Models
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public long ProductId { get; set; }
        public string ProductSku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
