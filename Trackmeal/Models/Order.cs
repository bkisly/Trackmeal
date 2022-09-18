namespace Trackmeal.Models
{
    public class Order
    {
        public long Id { get; set; }
        public Guid Token { get; set; }
        public DateTime DateOrdered { get; set; }
        public List<CartEntry> Entries { get; set; } = null!;
    }
}
