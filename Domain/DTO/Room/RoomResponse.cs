using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class RoomResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Address { get; set; }
        public string Description { get; set; } = string.Empty;
        public double RoomSize { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public Guid? FloorId { get; set; }
        public Guid RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Vacant;

        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(RoomResponse)) return false;

            RoomResponse room = (RoomResponse)obj;
            return Id == room.Id && Name == room.Name &&
                RoomTypeId == room.RoomTypeId &&
                FloorId == room.FloorId &&
                Price == room.Price &&
                Address == room.Address &&
                RoomSize == room.RoomSize &&
                Images == room.Images &&
                   Description == room.Description &&
                   Status == room.Status && CreatedTime == room.CreatedTime &&
                   CreatedBy == room.CreatedBy && ModifiedTime == room.ModifiedTime &&
                   ModifiedBy == room.ModifiedBy && Deleted == room.Deleted &&
                   DeletedTime == room.DeletedTime && DeletedBy == room.DeletedBy;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public RoomUpdateRequest ToRoomUpdateRequest()
        {
            return new RoomUpdateRequest()
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Address = Address,
                Description = Description,
                RoomSize = RoomSize,
                Images = Images,
                FloorId = FloorId,
                RoomTypeId = RoomTypeId,
                Status = Status,
                ModifiedTime = ModifiedTime,
                ModifiedBy = ModifiedBy
            };
        }


        public RoomDeleteRequest ToRoomDeleteRequest()
        {
            return new RoomDeleteRequest()
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
    public static class RoomExtensions
    {
        public static RoomResponse ToRoomResponse(this Domain.Models.Room room)
        {
            return new RoomResponse()
            {
                Id = room.Id,
                Name = room.Name,
                Price = room.Price,
                Address = room.Address,
                Description = room.Description,
                Images=room.Images,
                FloorId = room.FloorId,
                RoomTypeId = room.RoomTypeId,
                RoomSize=room.RoomSize,
                Status = room.Status,
                CreatedTime = room.CreatedTime,
                CreatedBy = room.CreatedBy,
                ModifiedTime = room.ModifiedTime,
                ModifiedBy = room.ModifiedBy,
                Deleted = room.Deleted,
                DeletedTime = room.DeletedTime,
                DeletedBy = room.DeletedBy
            };
        }
    }

