using Domain.DTO.AmenityRoom;
using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.Room;
using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<RoomAvailableResponse> GetAvailableRooms(RoomAvailableRequest roomRequest)
        {
            var Response = new RoomAvailableResponse();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
               {
                    new("@MinPrice", roomRequest.MaxPrice),
                    new("@MaxPrice", roomRequest.MaxPrice),
                    new("@name", roomRequest.Name),
                    new("@CheckIn", roomRequest.StartDate),
                    new("@CheckOut", roomRequest.EndDate),
                    new("@FloorId", roomRequest.FloorId),
                    new("@RoomTypeId", roomRequest.RoomTypeId)
               };
                var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetAvailableRooms, parameters);
                var roomlist = new List<RoomResponse>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var room = RowToRoom(row);
                    var roomResponse = room.ToRoomResponse();
                    roomlist.Add(roomResponse);
                }
                if (roomlist.Count > 0)
                {
                    Response.LstRoom = roomlist;
                    Response.TotalRoom = roomlist.Count;
                    Response.TotalOccupancy = Convert.ToInt32(dataTable.Rows[0]["TotalOccupancy"]); ;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("An error occurred while getting available rooms", ex);
            }
            return Response;
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
                    new("@name", roomRequest.Name),
                    new("@Status", roomRequest.Status),
                    new("@FloorId", roomRequest.FloorId),
                    new("@RoomTypeID", roomRequest.RoomTypeId)
                };

                // Gọi thủ tục lưu trữ để lấy dữ liệu
                var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetListRoom, parameters);
                var roomlist = new List<RoomResponse>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var room = RowToRoom(row);
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
                    new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = roomId }
                };

                var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_GetRoomById, parameters);

                if (dataTable.Rows.Count == 0)
                    return null;

                var row = dataTable.Rows[0];
                var room = RowToRoom(row); // Ánh xạ từ DataRow sang Room

                // Lấy danh sách tiện ích liên quan đến RoomType
                room.RoomType.AmenityRooms = await GetAmenityRoomsByRoomTypeId(room.RoomTypeId);

                return room;
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("An error occurred while retrieving the room", e);
            }
        }


        public async Task<List<AmenityRoom>> GetAmenityRoomsByRoomTypeId(Guid roomTypeId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new("@RoomTypeId", SqlDbType.UniqueIdentifier) { Value = roomTypeId }
                };

                var dataTable = await _worker.GetDataTableAsync
                    (StoredProcedureConstant.SP_GetAmenityRoomsByRoomTypeId, parameters);
                if (dataTable.Rows.Count == 0)
                {
                    return new List<AmenityRoom>(); // Không có dữ liệu trả về
                }

                var amenityRooms = new List<AmenityRoom>();

                foreach (DataRow row in dataTable.Rows)
                {
                    if (dataTable.Columns.Contains("Id"))
                    {
                        var amenityRoom = new AmenityRoom()
                        {
                            Id = (Guid)row["Id"],
                            RoomTypeId = (Guid)row["RoomTypeId"],
                            AmenityId = (Guid)row["AmenityId"],
                            Amount = (int)row["Amount"],
                            Status = (EntityStatus)row["Status"],
                            // Gọi thêm GetAmenityById để lấy thông tin về Amenity
                            Amenity = await GetAmenityById((Guid)row["AmenityId"])
                        };
                        amenityRooms.Add(amenityRoom);
                    }
                    else
                    {
                        throw new ArgumentException("Column 'Id' does not exist in the result set.");
                    }
                }

                return amenityRooms;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving the list amenity room", e);
            }
        }
        private async Task<Amenity?> GetAmenityById(Guid amenityId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new("@Id", SqlDbType.UniqueIdentifier) { Value = amenityId }
                };

                var dataTable = await _worker.GetDataTableAsync
                    (StoredProcedureConstant.SP_GetAmenityById, parameters);

                if (dataTable.Rows.Count == 0) return null;

                var row = dataTable.Rows[0];
                var amenity = new Amenity
                {
                    Id = (Guid)row["Id"],
                    Name = (string)row["Name"]
                };

                return amenity;
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("An error occurred while retrieving the amenity", e);
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
                new SqlParameter("@Deleted", SqlDbType.Bit) { Value = room.Deleted }
                };

                await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertRoom, parameters);

                return room;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("An error occurred while adding the room", e);
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
                throw new Exception("Some errors when deleted rooom", e);
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
                DeletedBy = ConvertGuidToString(row, "DeletedBy"),
                RoomType = new RoomType()
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

        public async Task<int> UpdateRoomStatus(RoomUpdateStatusRequest request)
        {
            var existingRoom = GetRoomById(request.Id);
            if (existingRoom == null)
            {
                throw new Exception("Customer could not be found");
            }
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@Status", SqlDbType.Int) { Value = request.Status != 0? request.Status : DBNull.Value },
                    new SqlParameter("@ModifiedTime",DateTime.Now),
                    new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
                };

                return _worker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateRoomStatus, sqlParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoomAvailableResponse> SearchRooms(SearchRoomsRequest request)
        {
            var Response = new RoomAvailableResponse();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new("@MinPrice", request.MaxPrice),
                    new("@MaxPrice", request.MaxPrice),
                    new("@MaximumOccupancy", request.MaxiumOccupancy),
                    new("@QuantityRoom", request.QuantityRoom),
                    new("@CheckIn", request.CheckIn),
                    new("@CheckOut", request.CheckOut),
                    new("@FloorId", request.FloorId)
               };
                var dataTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_SearchRooms, parameters);
                var roomlist = new List<RoomResponse>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var room = RowToRoom(row);
                    var roomResponse = room.ToRoomResponse();
                    roomlist.Add(roomResponse);
                }
                if (roomlist.Count > 0)
                {
                    Response.LstRoom = roomlist;
                    Response.TotalRoom = roomlist.Count;
                    Response.TotalOccupancy = Convert.ToInt32(dataTable.Rows[0]["TotalOccupancy"]); ;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("An error occurred while getting available rooms", ex);
            }
            return Response;
        }
        public async Task<List<TopRoomBookingViewModel>> GetTopBookingRoomsAsync(int SelectedMonthRoom, int SelectedYearRoom)
        {
            // Kiểm tra giá trị filterType hợp lệ
            if (SelectedMonthRoom < 1 || SelectedMonthRoom > 12)
            {
                throw new ArgumentException("Selected month must be between 1 and 12.", nameof(SelectedMonthRoom));
            }

            if (SelectedYearRoom < 2024 || SelectedYearRoom > DateTime.Now.Year)
            {
                throw new ArgumentException("Select only from 2024 to present", nameof(SelectedYearRoom));
            }

            // Tham số đầu vào cho stored procedure
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SelectedMonthRoom", SelectedMonthRoom),
                new SqlParameter("@SelectedYearRoom", SelectedYearRoom)
            };

            // Gọi stored procedure và lấy dữ liệu
            var dataTable = await _worker.GetDataTableAsync("SP_GetTop5MostBookedRoomsLastMonth", parameters);

            // Khởi tạo danh sách các phòng top booking
            var topRooms = new List<TopRoomBookingViewModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                var topRoom = new TopRoomBookingViewModel
                {
                    Id = Guid.Parse(row["RoomId"].ToString()),
                    Name = row["RoomName"]?.ToString(),
                    RoomTypeName = row["RoomTypeName"]?.ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    RoomSize = Convert.ToDouble(row["RoomSize"]),
                    Address = row["Address"]?.ToString(),
                    Images = row["Images"]?.ToString()?.Split(',').ToList() ?? new List<string>(),
                    BookingCount = Convert.ToInt32(row["BookingCount"])
                };

                topRooms.Add(topRoom);
            }

            return topRooms;
        }
        public async Task<List<TopCustomerBooking>> GetTopCustomerBookings(int SelectedMonthCustomer, int SelectedYearCustomer)
        {
            // Kiểm tra giá trị tháng và năm hợp lệ
            if (SelectedMonthCustomer < 1 || SelectedMonthCustomer > 12)
            {
                throw new ArgumentException("Selected month must be between 1 and 12.", nameof(SelectedMonthCustomer));
            }

            if (SelectedYearCustomer < 2024 || SelectedYearCustomer > DateTime.Now.Year)
            {
                throw new ArgumentException("Select only from 2024 to present", nameof(SelectedYearCustomer));
            }

            // Tham số đầu vào cho stored procedure
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SelectedMonthCustomer", SelectedMonthCustomer),
                new SqlParameter("@SelectedYearCustomer", SelectedYearCustomer)
            };

            // Gọi stored procedure và lấy dữ liệu
            var dataTable = await _worker.GetDataTableAsync("SP_GetTopMostBookedRoomsCustomer", parameters);

            // Khởi tạo danh sách các khách hàng top booking
            var topCustomers = new List<TopCustomerBooking>();
            foreach (DataRow row in dataTable.Rows)
            {
                var customer = new TopCustomerBooking
                {
                    Id = Guid.Parse(row["Id"].ToString()),
                    FirstName = row["FirstName"]?.ToString(),
                    LastName = row["LastName"]?.ToString(),
                    Email = row["Email"]?.ToString(),
                    PhoneNumber = row["PhoneNumber"]?.ToString(),
                    Gender = row["Gender"] != DBNull.Value ? Enum.Parse<GenderType>(row["Gender"].ToString()) : (GenderType?)null,
                    BookingCount = Convert.ToInt32(row["BookingCount"]),
                    TotalPrice = Convert.ToDecimal(row["TotalPrice"])
                };

                topCustomers.Add(customer);
            }

            return topCustomers;
        }

        public async Task<List<GetRevenue>> GetRevenueAsync(string revenueFilterType)
        {
            if (revenueFilterType != "Month" && revenueFilterType != "Year" && revenueFilterType != "month" && revenueFilterType != "year")
            {
                throw new ArgumentException("FilterType must be 'Month' or 'Year'.", nameof(revenueFilterType));
            }

            // Tham số đầu vào cho stored procedure
            SqlParameter[] parameters = new SqlParameter[] {
                new("@revenueFilterType", revenueFilterType)
            };

            // Gọi stored procedure và lấy dữ liệu
            var dataTable = await _worker.GetDataTableAsync("SP_GetRevenue", parameters);

            // Khởi tạo danh sách các phòng top booking
            var revenueList = new List<GetRevenue>();
            foreach (DataRow row in dataTable.Rows)
            {
                var revenue = new GetRevenue
                {
                    Period = row["Period"].ToString(), // Ánh xạ cột Period
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]) // Ánh xạ cột TotalAmount
                };

                revenueList.Add(revenue);
            }

            return revenueList;
        }
    }
}
