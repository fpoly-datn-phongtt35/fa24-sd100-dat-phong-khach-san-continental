using Domain.DTO.EditHistory;

namespace Domain.Services.IServices.IEditHistory;

public interface IEditHistoryAddService
{
    Task<EditHistoryResponse> AddEditHistoryService(EditHistoryCreateRequest editHistoryCreateRequest);
}