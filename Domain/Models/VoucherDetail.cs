using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class VoucherDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomBookingId { get; set; }
        public Guid VoucherId { get; set; }
        public string Code { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }
        public RoomBooking RoomBooking { get; set; }
        public Voucher Voucher { get; set; }
    }
}
