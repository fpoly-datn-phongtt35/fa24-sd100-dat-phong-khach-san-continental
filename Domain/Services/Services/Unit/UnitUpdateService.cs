using Domain.DTO.Unit;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IUnit;

namespace Domain.Services.Services.Unit;

public class UnitUpdateService : IUnitUpdateService
{
    private readonly IUnitRepository unitRepository;

    public UnitUpdateService(IUnitRepository unitRepository)
    {
        this.unitRepository = unitRepository;
    }

    public async Task<UnitResponse?> UpdateUnit(UnitUpdateRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));
        
        var existingUnit = await unitRepository.getUnitById(request.Id);
        if (existingUnit == null)
            throw new ArgumentException($"Id unit does not exist");
        
        existingUnit.Name = request.Name;
        existingUnit.Description = request.Description;
        existingUnit.Status = request.Status;
        
        await unitRepository.updateUnit(existingUnit);
        return existingUnit.ToUnitResponse();
    }
}