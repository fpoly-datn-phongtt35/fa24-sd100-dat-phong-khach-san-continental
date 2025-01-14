using Domain.DTO.EditHistory;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IEditHistory;

namespace Domain.Services.Services.EditHistory;

public class EditHistoryAddService : IEditHistoryAddService
{
    private readonly IEditHistoryRepository _editHistoryRepository;

    public EditHistoryAddService(IEditHistoryRepository editHistoryRepository)
    {
        _editHistoryRepository = editHistoryRepository;
    }

    public async Task<EditHistoryResponse> AddEditHistoryService(EditHistoryCreateRequest editHistoryCreateRequest)
    {
        if(editHistoryCreateRequest is null)
            throw new ArgumentNullException(nameof(editHistoryCreateRequest));
        
        var editHistory = editHistoryCreateRequest.ToEditHistory();
        editHistory.ModifiedAt = editHistory.ModifiedAt;
        editHistory.Content = editHistoryCreateRequest.Content;
        editHistory.Description = editHistoryCreateRequest.Description;
        
        await _editHistoryRepository.AddEditHistory(editHistory);
        return editHistory.ToEditHistoryResponse();
    }
}