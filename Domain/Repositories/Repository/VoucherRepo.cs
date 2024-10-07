using Domain.DTO.Voucher;
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
	public class VoucherRepo : IVoucherRepo
	{
		private static DbWorker _DbWorker;
		private readonly IConfiguration _configuration;
		public VoucherRepo(IConfiguration configuration)
		{
			_DbWorker = new DbWorker(StoredProcedureConstant.Continetal);
			_configuration = configuration;
		}

		public async Task<int> AddVoucher(VoucherCreateRequest request)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Name", request.Name),
					new SqlParameter("@Description", request.Description),
					new SqlParameter("@DiscountType", request.DiscountType),
					new SqlParameter("@DiscountValue", request.DiscountValue),
					new SqlParameter("@Status", (int)request.Status),
					new SqlParameter("@CreatedTime", request.CreatedTime),
					new SqlParameter("@CreatedBy", request.CreatedBy != null ? request.CreatedBy : DBNull.Value)
				};

				return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_InsertVoucher, sqlParameters);
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
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Id", request.Id),
					new SqlParameter("@DeletedTime", DateTime.Now),
					new SqlParameter("@DeletedBy", request.DeletedBy != Guid.Empty ? (object)request.DeletedBy : DBNull.Value)
				};

				return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_DeleteVoucher, sqlParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<DataTable> GetAllVoucher(VoucherGetRequest Voucher)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
				new SqlParameter("@DiscountType", Voucher.DiscountType != null ? (int)Voucher.DiscountType : (object)DBNull.Value),
				new SqlParameter("@PageSize", Voucher.PageSize),
				new SqlParameter("@PageIndex", Voucher.PageIndex)
				};

				return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetListVoucher, sqlParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<DataTable> GetVoucherById(Guid id)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Id", id != null ? id : DBNull.Value ),
				};

				return _DbWorker.GetDataTable(StoredProcedureConstant.SP_GetVoucherById, sqlParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<int> UpdateVoucher(VoucherUpdateRequest request)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter("@Id", request.Id),
					new SqlParameter("@Name", request.Name),
					new SqlParameter("@Description", request.Description),
					new SqlParameter("@DiscountType", request.DiscountType),
					new SqlParameter("@DiscountValue", request.DiscountValue),
					new SqlParameter("@Status", (int)request.Status),
					new SqlParameter("@Deleted", request.Deleted),
                    new SqlParameter("@ModifiedTime",DateTime.Now),
					new SqlParameter("@ModifiedBy", request.ModifiedBy!= null ? request.ModifiedBy : DBNull.Value)
				};

				return _DbWorker.ExecuteNonQuery(StoredProcedureConstant.SP_UpdateVoucher, sqlParameters);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
