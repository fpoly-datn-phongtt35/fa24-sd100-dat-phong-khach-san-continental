namespace ViewClient.Models.DTO
{
    public class ServiceOrderDetailCreateRequest
    {
        public Guid ServiceId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount => Price * Quantity;
    }
}
