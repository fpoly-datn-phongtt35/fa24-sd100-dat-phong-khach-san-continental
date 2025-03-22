using Domain.DTO.Feedback;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;
        private readonly IConfiguration _configuration;
        public FeedbackService(IConfiguration configuration, IFeedbackRepository repo)
        {
            _repo = repo;
            _configuration = configuration;
        }
        public Task<int> AddFeedback(FeedbackCreateRequest request)
        {
            try
            {
                return _repo.AddFeedback(request);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<int> DeleteFeedback(FeedbackDeleteRequest request)
        {
            try
            {
                return _repo.DeleteFeedback(request);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<FeedbackDto> GetFeedbackById(Guid id)
        {
            FeedbackDto fb = new();
            try
            {
                DataTable table = await _repo.GetFeedbackById(id);
                fb = (from row in table.AsEnumerable()
                    select new FeedbackDto()
                    {
                        Id = row.Field<Guid?>("Id") != null ? row.Field<Guid>("Id") : Guid.Empty,
                        RoomBookingDetailId = row.Field<Guid?>("RoomBookingDetailId") != null ? row.Field<Guid>("RoomBookingDetailId") : Guid.Empty,
                        Comments = row.Field<string>("Comments"),
                        Rating = row.Field<int>("Rating"),
                        Status = row.Field<EntityStatus>("Status"),
                        CreatedTime = row.IsNull("CreatedTime") ? (DateTimeOffset?)null : row.Field<DateTimeOffset>("CreatedTime"),
                        CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                        ModifiedTime = row.IsNull("ModifiedTime") ? (DateTimeOffset?)null : row.Field<DateTimeOffset>("ModifiedTime"),
                        ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                        Deleted = row.Field<bool>("Deleted"),
                        DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                        DeletedTime = row.IsNull("DeletedTime") ? (DateTimeOffset?)null : row.Field<DateTimeOffset>("DeletedTime"),
                        RoomId = row.Field<Guid?>("RoomId") != null ? row.Field<Guid>("RoomId") : Guid.Empty,
                        CustomerId = row.Field<Guid?>("CustomerId") != null ? row.Field<Guid>("CustomerId") : Guid.Empty
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return fb;
        }

        public async Task<ResponseData<FeedbackDto>> GetListFeedback(FeedbackGetRequest request)
        {
            var model = new ResponseData<FeedbackDto>();
            try
            {
                DataTable table = await _repo.GetAllFeedbacks(request);

                model.data = (from row in table.AsEnumerable()
                              select new FeedbackDto
                              {
                                  Id = row.Field<Guid?>("Id") != null ? row.Field<Guid>("Id") : Guid.Empty,
                                  RoomBookingDetailId = row.Field<Guid>("RoomBookingDetailId"),
                                  Comments = row.Field<string>("Comments"),
                                  Rating = row.Field<int>("Rating"),
                                  Status = row.Field<EntityStatus>("Status"),
                                  CreatedTime = row.IsNull("CreatedTime") ? null : row.Field<DateTimeOffset>("CreatedTime"),
                                  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
                                  ModifiedTime = row.IsNull("ModifiedTime") ? null : row.Field<DateTimeOffset>("ModifiedTime"),
                                  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
                                  Deleted = row.Field<bool>("Deleted"),
                                  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
                                  DeletedTime = row.IsNull("DeletedTime") ? null : row.Field<DateTimeOffset>("DeletedTime"),
                                  RoomId = row.Field<Guid?>("RoomId") != null ? row.Field<Guid>("RoomId") : Guid.Empty,
                                  CustomerId = row.Field<Guid?>("CustomerId") != null ? row.Field<Guid>("CustomerId") : Guid.Empty

                              }).ToList();

                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;

                try
                {
                    model.totalRecord = Convert.ToInt32(table.Rows[0]["TotalRows"]);
                }
                catch (Exception ex)
                {
                    model.totalRecord = 0;
                }

                //tổng trang
                model.totalPage = (int)Math.Ceiling((double)model.totalRecord / request.PageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }

        public Task<int> UpdateFeedback(FeedbackUpdateRequest request)
        {
            try
            {
                return _repo.UpdateFeedback(request);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
