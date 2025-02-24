using Domain.DTO.Unit;

namespace Domain.Services.IServices.IUnit;

public interface IUnitUpdateService
{
    Task<UnitResponse?> UpdateUnit(UnitUpdateRequest request);
}