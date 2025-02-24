using Domain.DTO.Paging;
using Domain.DTO.Unit;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IUnit;

namespace Domain.Services.Services.Unit;

public class UnitGetService : IUnitGetService
{
    private readonly IUnitRepository unitRepository;

    public UnitGetService(IUnitRepository unitRepository)
    {
        this.unitRepository = unitRepository;
    }

    public async Task<UnitResponse?> GetUnitById(Guid? id)
    {
        if (id == null)
            return null;

        var unit = await unitRepository.getUnitById(id.Value);
        if (unit == null)
            return null;
        return unit.ToUnitResponse();
    }

    public async Task<ResponseData<UnitResponse>> GetFilteredUnits(UnitGetRequest request)
    {
        return await unitRepository.getFilteredUnits(request);
    }
}