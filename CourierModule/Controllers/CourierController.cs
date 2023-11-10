using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sectors_service_orders.Auth;
using sectors_srv_manifest.AuthModule.Exceptions;
using sectors_srv_manifest.AuthModule.Models;
using sectors_srv_manifest.CourierModule.Exceptions;
using sectors_srv_manifest.CourierModule.Models.Reqs;
using sectors_srv_manifest.CourierModule.Services;

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

    [HttpGet("{courierId}")]
    public async Task<IActionResult> GetCourier(int courierId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var courier = await courierService.GetSingleCourier(courierId, authData.ClientId);
            if (courier == null)
            {
                return NotFound();
            }
            return Ok(courier);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpGet()]
    public async Task<IActionResult> GetCouriers([FromQuery] CourierFiltersReq filters)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var (couriers, totalCount) = await courierService.GetManyCouriers(filters, authData.ClientId);
            return Ok(new { couriers, totalCount });
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpPut("{courierId?}")]
    public async Task<IActionResult> UpdateCourier( UpdateCourierReq data, int? courierId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        if (courierId.HasValue)
        {
            data.Id = courierId.Value; 
        }
        try
        {
            var courier = await courierService.UpdateCourier(data, authData.ClientId, authData.UserId);
            if (courier == null)
            {
                return NotFound();
            }
            return Ok(courier);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpDelete("{courierId}")]
    public async Task<IActionResult> SoftDeleteCourier(int courierId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            await courierService.SoftDeleteCourier(courierId, authData.ClientId, authData.UserId);
            return Ok($"Se ha eliminado el Courier con codigo {courierId}");
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

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
