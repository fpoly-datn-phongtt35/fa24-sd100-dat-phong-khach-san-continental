using Domain.DTO.Unit;

namespace Domain.Services.IServices.IUnit;

public interface IUnitDeleteService
{
    Task<UnitResponse?> DeleteUnitById(UnitDeleteRequest request);  
}