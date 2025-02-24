using Domain.DTO.Paging;
using Domain.DTO.Unit;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Services.IServices.IUnit;

public interface IUnitGetService
{
    Task<UnitResponse?> GetUnitById(Guid? id);
    Task<ResponseData<UnitResponse>> GetFilteredUnits(UnitGetRequest request);
}