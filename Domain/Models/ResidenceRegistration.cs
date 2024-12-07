using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ResidenceRegistration
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RoomBookingDetailId{ get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderType? Gender { get; set; }
        public string? IdentityNumber { get; set; }
        public string? PhoneNumber { get; set; }
        //public string? Nationality { get; set; }
        //public string? PassPortNumber { get; set; }
        //public DateTime? EntryDate { get; set; }
        //public DateTime? PortOfEntry { get; set; }
        //public DateTime? EntryPermitNumber { get; set; }
        //public string? VisaNumber { get; set; }
        //public DateTime? VisaIssueDate { get; set; }
        //public DateTime? VisaExpireDate { get; set; }

        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public virtual RoomBookingDetail RoomBookingDetail { get; set; }
    }
}
