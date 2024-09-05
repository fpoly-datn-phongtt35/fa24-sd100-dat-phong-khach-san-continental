﻿using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ServiceOrderDetail
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Amount { get; set; }
        public Guid ServiceId { get; set; }
        public Guid ServiceOrderId { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }

        public Service Service { get; set; }
        public ServiceOrder ServiceOrder { get; set; }
    }
}