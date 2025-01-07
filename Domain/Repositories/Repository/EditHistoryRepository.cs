using System.Data;
using Domain.Models;
using Domain.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Utilities.StoredProcedure;

namespace Domain.Repositories.Repository;

public class EditHistoryRepository : IEditHistoryRepository
{
    private readonly DbWorker _worker;
    private readonly IConfiguration _configuration;

    public EditHistoryRepository(IConfiguration configuration)
    {
        _worker = new DbWorker(StoredProcedureConstant.Continetal);
        _configuration = configuration;
    }
    
    public async Task<EditHistory> AddEditHistory(EditHistory editHistory)
    {
        try
        {
            SqlParameter[] parameters =
            {
                new("@RoomBookingDetailId", SqlDbType.UniqueIdentifier) { Value = editHistory.RoomBookingDetailId },
                new("@For", SqlDbType.Int) { Value = editHistory.For },
                new("@Content", SqlDbType.NVarChar) { Value = editHistory.Content ?? (object)DBNull.Value },
                new("@Description", SqlDbType.NVarChar) { Value = editHistory.Description ?? (object)DBNull.Value },
                new("@ModifiedAt", SqlDbType.DateTimeOffset) { Value = editHistory.ModifiedAt ?? DateTimeOffset.Now }
            };
     
            var resultTable = await _worker.GetDataTableAsync(StoredProcedureConstant.SP_InsertEditHistory, parameters);
            if (resultTable.Rows.Count > 0)
                editHistory.Id = Convert.ToInt32(resultTable.Rows[0]["InsertedId"]);

            return editHistory;
        }
        catch (SqlException sqlEx)
        {
            // Kiểm tra mã lỗi từ exception của SQL Server
            if (sqlEx.Number == 50002)  // Lỗi khi bản ghi đã tồn tại
            {
                throw new Exception("Lịch sử chỉnh sửa đã tồn tại cho bản ghi này.");
            }

            Console.WriteLine($"Lỗi khi thêm EditHistory: {sqlEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("An error occurred while adding the EditHistory.", ex);
        }
    }

    public Task<EditHistory?> UpdateEditHistory(EditHistory editHistory)
    {
        throw new NotImplementedException();
    }

    public Task<EditHistory?> GetEditHistoryById(int editHistoryId)
    {
        throw new NotImplementedException();
    }
}