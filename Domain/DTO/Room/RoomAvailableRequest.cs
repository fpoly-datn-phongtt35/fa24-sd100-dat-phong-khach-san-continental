using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomAvailableRequest
    {
        public string? Name { get; set; }
        public Guid? FloorId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal? MinPrice {  get; set; }
        public decimal? MaxPrice { get; set;}
    }
}
