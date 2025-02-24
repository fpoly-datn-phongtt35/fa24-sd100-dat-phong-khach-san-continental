using Domain.DTO.Unit;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IUnit;

namespace Domain.Services.Services.Unit;

public class UnitDeleteService : IUnitDeleteService
{
    private readonly IUnitRepository unitRepository;

    public UnitDeleteService(IUnitRepository unitRepository)
    {
        this.unitRepository = unitRepository;
    }

    public async Task<UnitResponse?> DeleteUnitById(UnitDeleteRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var existingUnit = await unitRepository.getUnitById(request.Id);
        if(existingUnit == null)
            throw new ArgumentException("Id unit not found");

        existingUnit.Status = (EntityStatus.Deleted);
        
        await unitRepository.deleteUnitById(existingUnit);
        return existingUnit.ToUnitResponse();
    }
}