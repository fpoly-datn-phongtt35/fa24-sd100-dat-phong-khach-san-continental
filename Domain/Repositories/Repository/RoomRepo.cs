using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository
{
    public class RoomRepo : IRoomRepo
    {
        private readonly DbWorker _worker;
        private readonly IConfiguration _configuration;

        public RoomRepo(IConfiguration configuration)
        {
            _worker = new DbWorker(StoredProcedureConstant.Continetal);
            _configuration = configuration;
        }

        public async Task<ResponseData<RoomResponse>> GetAllRooms(RoomRequest roomRequest)
        {
            var rooms = new ResponseData<RoomResponse>();
            try
            {

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new("@PageIndex", roomRequest.PageIndex),
                    new("@PageSize", roomRequest.PageSize),
                    new("@Name", roomRequest.Name),
                    new("@Status", roomRequest.Status),
                    new("@FloorId", roomRequest.FloorId),
                    new("@RoomTypeID", roomRequest.RoomTypeId)
                };

                // Gọi thủ tục lưu trữ để lấy dữ liệu
                var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetListRoom, parameters);
                var roomlist = new List<RoomResponse>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var room =RowToRoom(row);
                    var roomResponse = room.ToRoomResponse();
                    roomlist.Add(roomResponse);
                }
                try
                {
                    rooms.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
                }
                catch (Exception e)
                {
                    rooms.totalRecord = 0;
                }
                rooms.totalPage = (int)Math.Ceiling((double)rooms.totalRecord / roomRequest.PageSize);
                rooms.data = roomlist;
                rooms.CurrentPage = roomRequest.PageIndex;
                rooms.PageSize = roomRequest.PageSize;
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("An error occurred while getting all room", e);
            }
            return rooms;
        }

        public async Task<Room?> GetRoomById(Guid roomId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value =roomId }
                };

                var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetRoomById, parameters);

                if (dataTable.Rows.Count == 0)
                    return null;

                var row = dataTable.Rows[0];
                var room = RowToRoom(row);

                return room;
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("An error occurred while retrieving the room", e);
            }
        }

        public async Task<Room> AddRoom(Room room)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@FloorId", SqlDbType.UniqueIdentifier) { Value = room.FloorId },
                new SqlParameter("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = room.RoomTypeId },
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = room.Name },
                new SqlParameter("@Price", SqlDbType.Decimal) { Value = room.Price },
                new SqlParameter("@Address", SqlDbType.NVarChar) { Value = room.Address },
                new SqlParameter("@RoomSize", SqlDbType.Float) { Value = room.RoomSize },
                new SqlParameter("@Images", SqlDbType.NVarChar) { Value = string.Join(",", room.Images) },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = room.Description },
                new SqlParameter("@Status", SqlDbType.Int) { Value = room.Status },
                new SqlParameter("@CreatedTime", SqlDbType.DateTimeOffset) { Value = room.CreatedTime },
                new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier) { Value = room.CreatedBy },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = room.ModifiedTime },
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = room.Deleted },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = room.DeletedTime }
                };

                await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertRoom, parameters);

                return room;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("An error occurred while adding the amenity", e);
            }
        }

        public async Task<Room?> UpdateRoom(Room room)
        {
            try
            {
                var existingRoom = GetRoomById(room.Id);
                if (existingRoom == null)
                {
                    throw new Exception("There is no amenity room with the provided Id.");
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = room.Id },
                new SqlParameter("@FloorId", SqlDbType.UniqueIdentifier) { Value = room.FloorId },
                new SqlParameter("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = room.RoomTypeId },
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = room.Name },
                new SqlParameter("@Price", SqlDbType.Decimal) { Value = room.Price },
                new SqlParameter("@Address", SqlDbType.NVarChar) { Value = room.Address },
                new SqlParameter("@RoomSize", SqlDbType.Float) { Value = room.RoomSize },
                new SqlParameter("@Images", SqlDbType.NVarChar) { Value = string.Join(",", room.Images) },
                new SqlParameter("@Description", SqlDbType.NVarChar) { Value = room.Description },
                new SqlParameter("@Status", SqlDbType.Int) { Value = room.Status },
                new SqlParameter("@Deleted",SqlDbType.Int) { Value = room.Deleted },
                new SqlParameter("@ModifiedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = room.ModifiedBy }
                };
                await _worker.GetDataTableAsync(StoredProcedureConstant.SP_UpdateRoom, parameters);

                return await existingRoom;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while updating the room", e);
            }
        }

        public async Task<Room?> DeleteRoom(Room room)
        {
            try
            {
                var existingRoom = GetRoomById(room.Id);
                if (existingRoom == null)
                {
                    throw new Exception("There is no room with the provided Id.");
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = room.Id },
                new SqlParameter("@DeletedTime", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                new SqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier) { Value = room.DeletedBy },
                };

                await _worker.GetDataTableAsync(StoredProcedureConstant.SP_DeleteRoom, parameters);

                return await existingRoom;
            }
            catch (Exception e)
            {
                throw new Exception("Some errors when deleted amenity rooom", e);
            }
        }

        private Room RowToRoom(DataRow row)
        {
            return new Room()
            {
                Id = Guid.Parse(row["Id"].ToString()!),
                Name = row["Name"].ToString()!,
                FloorId = Guid.Parse(row["FloorId"].ToString()!),
                RoomTypeId = Guid.Parse(row["RoomTypeId"].ToString()!),
                Price = decimal.Parse(row["Price"].ToString()!),
                Address = row["Address"].ToString()!,
                RoomSize = double.Parse(row["RoomSize"].ToString()!),
                Images = row["Images"].ToString()!.Split(',').ToList(),
                Description = row["Description"].ToString()!,
                Status = row.Table.Columns.Contains("Status") ? (RoomStatus)int.Parse(row["Status"].ToString()!) : default,
                CreatedTime = ConvertDateTimeOffsetToString(row, "CreatedTime"),
                CreatedBy = ConvertGuidToString(row, "CreatedBy"),
                ModifiedTime = ConvertDateTimeOffsetToString(row, "ModifiedTime"),
                ModifiedBy = ConvertGuidToString(row, "ModifiedBy"),
                Deleted = row["Deleted"] != DBNull.Value && (bool)row["Deleted"],
                DeletedTime = ConvertDateTimeOffsetToString(row, "DeletedTime"),
                DeletedBy = ConvertGuidToString(row, "DeletedBy")
            };
        }

        private static DateTimeOffset ConvertDateTimeOffsetToString(DataRow row, string columnName)
        {
            if (row[columnName] != DBNull.Value)
            {
                return DateTimeOffset.Parse(row[columnName].ToString()!);
            }
            return DateTimeOffset.MinValue;
        }

        private static Guid? ConvertGuidToString(DataRow row, string columnName)
        {
            if (row[columnName] != DBNull.Value)
            {
                return Guid.Parse(row[columnName].ToString()!);
            }
            return null;
        }

    }
}
