using Domain.DTO.Amenity;
using Domain.DTO.Paging;
using Domain.DTO.Unit;
using Domain.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Repositories.IRepository;

public interface IUnitRepository
{
    Task<Unit> addUnit(Unit unit);
    
    Task<Unit?> updateUnit(Unit unit);
    
    Task<Unit?> deleteUnitById(Unit unit);

    Task<Unit?> getUnitById(Guid id);

    Task<Unit?> getUnitExists(string name);
    
    Task<ResponseData<UnitResponse>> getFilteredUnits(UnitGetRequest unitGetRequest);
}