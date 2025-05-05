using Domain.Enums;
using Domain.Models;

namespace View.Models.RoomBooking
{
    public class RoomBookingPdfDto
    {
        public Guid Id { get; set; }  
        public Guid CustomerId { get; set; }  
        public string? CustomerName { get; set; }  
        public RoomBookingStatus Status { get; set; } 
        public decimal? TotalRoomPrice { get; set; } 
        public decimal? TotalServicePrice { get; set; } 
        public decimal? TotalExtraPrice { get; set; } 
        public decimal? TotalPriceReality { get; set; }  
        public DateTimeOffset? CreatedTime { get; set; }  
        public List<RoomBookingDetailPdfDto> RoomDetails { get; set; }
    }

    public class RoomBookingDetailPdfDto
    {
        public Guid Id { get; set; }  
        public string RoomName { get; set; }  
        public decimal RoomPrice { get; set; }  
        public DateTimeOffset? CheckInReality { get; set; }  
        public DateTimeOffset? CheckOutReality { get; set; } 
        public decimal? Expenses { get; set; }  
        public EntityStatus Status { get; set; }
        public List<ServiceOrderDetailPdfDto> ServiceDetails { get; set; }
    }

    public class ServiceOrderDetailPdfDto
    {
        public Guid Id { get; set; }  
        public string ServiceName { get; set; }  
        public decimal Price { get; set; } 
        public int Quantity { get; set; }  
        public decimal TotalPrice { get; set; } 
    }
}
