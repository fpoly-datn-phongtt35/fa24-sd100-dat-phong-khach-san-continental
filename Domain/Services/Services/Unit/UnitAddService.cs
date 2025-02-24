using Domain.DTO.Unit;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IUnit;

namespace Domain.Services.Services.Unit;

public class UnitAddService : IUnitAddService
{
    private readonly IUnitRepository unitRepository;

    public UnitAddService(IUnitRepository unitRepository)
    {
        this.unitRepository = unitRepository;
    }

    public async Task<UnitResponse> AddUnit(UnitCreateRequest request)
    {
        if(request is null)
            throw new ArgumentNullException(nameof(request));
        
        var exists = await unitRepository.getUnitExists(request.Name);
        if (exists != null)
            throw new Exception($"Đã tồn tại đơn vị với tên {request.Name}");

        var unit = request.ToUnit();
        
        await unitRepository.addUnit(unit);
        return unit.ToUnitResponse();
    }
}