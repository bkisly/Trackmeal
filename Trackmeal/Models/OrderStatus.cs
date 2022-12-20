namespace Trackmeal.Models
{
    public class OrderStatus
    {
        public byte Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public enum OrderStatusEnum : byte
    {
        Submitted = 1,
        InPreparation,
        ReadyToCollect,
        Completed
    }
}
