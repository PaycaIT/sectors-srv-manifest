using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sectors_service_orders.Auth;
using sectors_srv_manifest.AuthModule.Exceptions;
using sectors_srv_manifest.AuthModule.Models;
using sectors_srv_manifest.CourierModule.Controllers;
using sectors_srv_manifest.CourierModule.Models.Reqs;
using sectors_srv_manifest.TrackingModule.Exceptions;
using sectors_srv_manifest.TrackingModule.Models.Reqs;
using sectors_srv_manifest.TrackingModule.Services;

namespace sectors_srv_manifest.TrackingModule.Controllers;


[ApiController]
[Route("v1/tracking")]
[Authorize]
public class TrackingController : Controller
{
    private readonly TrackingService trackingService = new();
    private readonly ILogger<CourierController> logger;

    public TrackingController(ILogger<CourierController> _logger)
    {
        logger = _logger;
    }


    [HttpPost()]
    public async Task<IActionResult> CreateCourier(CreateSOTrackingReq data)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var SOTracking = await trackingService.CreateSOTracking(data, authData.ClientId, authData.UserId);
            return Ok(SOTracking);
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
