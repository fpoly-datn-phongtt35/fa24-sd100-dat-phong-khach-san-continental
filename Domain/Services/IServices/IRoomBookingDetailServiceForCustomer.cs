using Domain.DTO.RoomBookingDetail;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IRoomBookingDetailServiceForCustomer
    {
        Task<RoomBookingDetail> GetById(Guid id);
        Task<int> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request);
        Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request);
        Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request);
    }
}
