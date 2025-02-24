using Domain.DTO.Unit;

namespace Domain.Services.IServices.IUnit;

public interface IUnitAddService
{
    Task<UnitResponse> AddUnit(UnitCreateRequest request);
}