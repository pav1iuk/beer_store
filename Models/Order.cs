namespace BeerStore.Api.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public long CustomerId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "NEW";
        public List<OrderItem> Items { get; set; } = new();
    }
}
