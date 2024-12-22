using Domain.DTO.RoomBookingDetail;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoomBooking
    {
        [Key]
        public Guid Id { get; set; }
        public BookingType? BookingType { get; set; } 
        public Guid CustomerId { get; set; }
        public Guid? StaffId { get; set; }
        public decimal? TotalPrice { get; set; }  
        public decimal? TotalPriceReality { get; set; }  
        public decimal? TotalRoomPrice { get; set; }  
        public decimal? TotalServicePrice { get; set; }  
        public decimal? TotalExtraPrice { get; set; }  
        public decimal? TotalExpenses { get; set; } 
        public RoomBookingStatus Status { get; set; } = RoomBookingStatus.PENDING;
        public BookingBy? BookingBy { get; set; }






        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Customer Customer { get; set; }
        public Staff Staff { get; set; }
        public List<RoomBookingDetail> RoomBookingDetails { get; set; }
        public ICollection<FeedBack> FeedBacks { get; set; }
        public List<VoucherDetail> VoucherDetails { get; set; }
        public List<ServiceOrderDetail> ServiceOrderDetails { get; set; }
        public virtual List<PaymentHistory> PaymentHistorys { get; set; }
    }
}
