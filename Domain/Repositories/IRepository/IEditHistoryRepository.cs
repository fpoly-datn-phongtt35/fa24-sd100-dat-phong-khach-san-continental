using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IEditHistoryRepository
{
    Task<EditHistory> AddEditHistory(EditHistory editHistory);
    Task<EditHistory?> UpdateEditHistory(EditHistory editHistory);
    Task<EditHistory?> GetEditHistoryById(int editHistoryId);
}