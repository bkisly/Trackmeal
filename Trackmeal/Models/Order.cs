namespace Trackmeal.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public Guid Token { get; set; }
        public DateTime DateOrdered { get; set; }

        public List<CartEntry> Entries { get; set; } = null!;

        public byte OrderStatusId { get; set; } = (byte)OrderStatusEnum.Submitted;
        public OrderStatus OrderStatus { get; set; } = null!;

        public decimal TotalPrice => Entries.Sum(entry => entry.TotalPrice);
    }
}
