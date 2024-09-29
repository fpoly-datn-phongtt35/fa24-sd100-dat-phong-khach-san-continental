using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomDeleteRequest
    {
        public Guid Id { get; set; }
        public RoomStatus Status { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }

        public Models.Room ToRoom()
        {
            return new Models.Room()
            {
                Id = Id,
                Status = Status,
                Deleted = Deleted,
                DeletedTime = DeletedTime,
                DeletedBy = DeletedBy
            };
        }
    }
}
