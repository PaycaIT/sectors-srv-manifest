using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sectors_service_orders.Auth;
using sectors_srv_manifest.AuthModule.Exceptions;
using sectors_srv_manifest.AuthModule.Models;
using sectors_srv_manifest.CourierModule.Exceptions;
using sectors_srv_manifest.CourierModule.Models.Reqs;
using sectors_srv_manifest.CourierModule.Services;
using System.Text.Json;

namespace sectors_srv_manifest.CourierModule.Controllers;

[ApiController]
[Route("v1/courier")]
[Authorize]
public class CourierController : Controller
{
    private readonly ILogger<CourierController> logger;
    private readonly CourierService courierService = new ();

    public CourierController(ILogger<CourierController> _logger)
    {
        logger = _logger;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateCourier(CreateCourierReq data)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var courier = await courierService.CreateCourier(data, authData.ClientId, authData.UserId);
            return Ok(courier);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingleManifest(int id)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var manifest = await manifestService.GetSingleManifest(id, authData.ClientId);
            return Ok(manifest);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpPost("find")]
    public async Task<IActionResult> GetManyManifest(ManifestFiltersReqModel filters)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var paginatedResponse = await manifestService.GetManyManifest(filters, authData.ClientId);
            return Ok(paginatedResponse);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManifest(int id, [FromBody] UpdateManifestReqModel data)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        data.Id = id;
        try
        {
            var modifiedManifest = await manifestService.UpdateManifest(data, authData.ClientId, authData.UserId);
            return Ok(modifiedManifest);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDeleteManifest(int id)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            await manifestService.SoftDeleteManifest(id, authData.ClientId, authData.UserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpPost("smart-associate")]
    public async Task<IActionResult> SmartAssociate(SmartAssociateManifestReq data)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var manifest = await manifestService.SmartAssociate(data.ServiceOrderExternalId, data.SectorId, authData.ClientId, authData.UserId, data.Force);
            return Ok(manifest);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    //public async Task<IActionResult> AddServiceOrderToManifest([FromBody] string serviceOrderExternalId, [FromBody] bool force = false)
    //{

    //}

    //public async Task<IActionResult> RemoveServiceOrderFromManifest()
    //{

    //}

    private IActionResult MapExceptionsToHttp(Exception ex)
    {
        if (ex is EntityNotFoundException notFoundEx)
        {
            return NotFound(new { message = notFoundEx.Message });
        }

        else if (ex is ForbiddenClientException forbiddenEx)
        {
            logger.LogCritical(ex, "Forbidden client error occurred.");
            return NotFound();
        }
        else if (ex is ArgumentException argumentEx)
        {
            return BadRequest(new { message = argumentEx.Message });
        }
        else if (ex is AuthException authEx)
        {
            return Unauthorized(new { message = authEx.Message });
        }
        else if (ex is NotImplementedException notImpException)
        {
            return NotFound(new { message = notImpException.Message });
        }
        else
        {
            logger.LogError(ex, "An internal server error occurred.");
            return StatusCode(500, new { message = "Internal Server Error" });
        }
    }
}
