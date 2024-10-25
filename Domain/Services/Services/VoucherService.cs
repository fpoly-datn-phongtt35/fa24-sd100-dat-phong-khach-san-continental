using Domain.DTO.Paging;
using Domain.DTO.Voucher;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace Domain.Services.Services
{
	public class VoucherService : IVoucherService
	{
		private readonly VoucherRepo _voucherRepo;
		private readonly IConfiguration _configuration;

		public VoucherService(VoucherRepo voucherRepo, IConfiguration configuration)
		{
			_voucherRepo = voucherRepo;
			_configuration = configuration;
		}

		public async Task<int> AddVoucher(VoucherCreateRequest request)
		{
			try
			{
				return await _voucherRepo.AddVoucher(request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<int> DeleteVoucher(VoucherDeleteRequest request)
		{
			try
			{
				return await _voucherRepo.DeleteVoucher(request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<ResponseData<Voucher>> GetAllVoucher(VoucherGetRequest Voucher)
		{
			var model = new ResponseData<Voucher>();
			try
			{
				DataTable dataTable = await _voucherRepo.GetAllVoucher(Voucher);
				model.data = (from row in dataTable.AsEnumerable()
							  select new Voucher
							  {
								  Id = row.Field<Guid>("Id"),
								  Name = row.Field<string>("Name"),
								  Description = row.Field<string>("Description"),
								  DiscountType = row.Field<DiscountType>("DiscountType"),
								  DiscountValue = row.Field<decimal>("DiscountValue"),
								  Status = row.Field<EntityStatus>("Status"),
								  CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
								  CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
								  ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
								  ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
								  Deleted = row.Field<bool>("Deleted"),
								  DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
								  DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
							  }).ToList();
				model.CurrentPage = Voucher.PageIndex;
				model.PageSize = Voucher.PageSize;
				try
				{
					// Thử chuyển đổi và gán giá trị
					model.totalRecord = Convert.ToInt32(dataTable.Rows[0]["TotalRows"]);
				}
				catch (Exception ex)
				{
					// Nếu có lỗi xảy ra (ví dụ: không tìm thấy cột, không thể chuyển đổi), gán giá trị mặc định là 0
					model.totalRecord = 0;
				}
				model.totalPage = (int)Math.Ceiling((double)model.totalRecord / Voucher.PageSize);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return model;
		}

		public async Task<Voucher> GetVoucherById(Guid id)
		{
			Voucher voucher = new Voucher();
			try
			{
				DataTable table = await _voucherRepo.GetVoucherById(id);
				voucher = (from row in table.AsEnumerable()
							select new Voucher
							{
								Id = row.Field<Guid>("Id"),
								Name = row.Field<string>("Name"),
								Description = row.Field<string>("Description"),
								DiscountType = row.Field<DiscountType>("DiscountType"),
								DiscountValue = row.Field<decimal>("DiscountValue"),
								Status = row.Field<EntityStatus>("Status"),
								CreatedTime = row.Field<DateTimeOffset>("CreatedTime"),
								CreatedBy = row.Field<Guid?>("CreatedBy") != null ? row.Field<Guid>("CreatedBy") : Guid.Empty,
								ModifiedTime = row.Field<DateTimeOffset>("ModifiedTime"),
								ModifiedBy = row.Field<Guid?>("ModifiedBy") != null ? row.Field<Guid>("ModifiedBy") : Guid.Empty,
								Deleted = row.Field<bool>("Deleted"),
								DeletedBy = row.Field<Guid?>("DeletedBy") != null ? row.Field<Guid>("DeletedBy") : Guid.Empty,
								DeletedTime = row.Field<DateTimeOffset>("DeletedTime")
							}).FirstOrDefault();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return voucher;
		}

		public async Task<int> UpdateVoucher(VoucherUpdateRequest request)
		{
			try
			{
				return await _voucherRepo.UpdateVoucher(request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
