using Domain.DTO.Paging;
using Domain.DTO.Unit;
using Domain.Services.IServices.IUnit;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnitController : Controller
{
    private readonly IUnitAddService unitAddService;
    private readonly IUnitDeleteService unitDeleteService;
    private readonly IUnitGetService unitGetService;
    private readonly IUnitUpdateService unitUpdateService;

    public UnitController(IUnitAddService unitAddService, IUnitDeleteService unitDeleteService, 
        IUnitGetService unitGetService, IUnitUpdateService unitUpdateService)
    {
        this.unitAddService = unitAddService;
        this.unitDeleteService = unitDeleteService;
        this.unitGetService = unitGetService;
        this.unitUpdateService = unitUpdateService;
    }

    [HttpPost(nameof(CreateUnit))]
    public async Task<IActionResult> CreateUnit(UnitCreateRequest request)
    {
        try
        {
            var response = await unitAddService.AddUnit(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet(nameof(GetFilteredUnits))]
    public async Task<ResponseData<UnitResponse>> GetFilteredUnits(UnitGetRequest request)
    {
        try
        {
            return await unitGetService.GetFilteredUnits(request);
        }
        catch (Exception ex)
        {
            throw new NullReferenceException("The list of units could not be retrieved", ex);
        }
    }

    [HttpGet(nameof(GetUnitById))]
    public async Task<UnitResponse?> GetUnitById(Guid id)
    {
        try
        {
            return await unitGetService.GetUnitById(id);
        }
        catch (Exception ex)
        {
            throw new NullReferenceException("Not found the unit", ex);
        }
    }

    [HttpPut(nameof(UpdateUnit))]
    public async Task<UnitResponse?> UpdateUnit(UnitUpdateRequest unitUpdateRequest)
    {
        try
        {
            return await unitUpdateService.UpdateUnit(unitUpdateRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    [HttpPut(nameof(DeleteUnit))]
    public async Task<UnitResponse?> DeleteUnit(UnitDeleteRequest unitDeleteRequest)
    {
        try
        {
            return await unitDeleteService.DeleteUnitById(unitDeleteRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Unit cannot be deleted", e);
        }
    }
}