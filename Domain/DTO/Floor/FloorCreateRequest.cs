using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Floor
{
    public class FloorCreateRequest
    {
        public Guid BuildingId { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; } = string.Empty;
        public int? NumberOfRoom { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;
        public DateTimeOffset? CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
