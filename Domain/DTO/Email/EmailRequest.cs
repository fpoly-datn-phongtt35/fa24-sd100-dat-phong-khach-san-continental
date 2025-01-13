namespace Domain.DTO.Email
{
    public class EmailRequest
    {
        public string ToEmail { get; set; } // Email người nhận
        public int EmailType { get; set; } // 1: Nhắc nhở, 2: Xác nhận
        public string? RoomDetails { get; set; }
        public string? BookingTime { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? PaidAmount { get; set; }
    }

}
