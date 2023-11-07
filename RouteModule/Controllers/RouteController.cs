using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sectors_service_orders.Auth;
using sectors_srv_manifest.AuthModule.Exceptions;
using sectors_srv_manifest.AuthModule.Models;
using sectors_srv_manifest.RouteModule.Exceptions;
using sectors_srv_manifest.RouteModule.Models.Reqs;
using sectors_srv_manifest.RouteModule.Services;

namespace sectors_srv_manifest.RouteModule.Controllers;

[ApiController]
[Route("v1/route")]
[Authorize]
public class RouteController : Controller
{
    private readonly ILogger<RouteController> logger;
    private readonly RouteService routeService = new();

    public RouteController(ILogger<RouteController> _logger)
    {
        logger = _logger;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateRoute(string startingManifestId, int courierId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var route = await routeService.CreateRoute(startingManifestId, courierId, authData.ClientId, authData.UserId);
            return Ok(route);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpGet("{routeId}")]
    public async Task<IActionResult> GetRoute(int routeId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var route = await routeService.GetSingleRoute(routeId, authData.ClientId);
            if (route == null)
            {
                return NotFound();
            }
            return Ok(route);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpGet()]
    public async Task<IActionResult> GetRoutes([FromQuery] RouteFiltersReq filters)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var (routes, totalCount) = await routeService.GetManyRoutes(filters, authData.ClientId);
            return Ok(new { routes, totalCount });
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpPut("{routeId?}")]
    public async Task<IActionResult> UpdateRoute(int? routeId)
    {
        int id = 0;
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        if (routeId.HasValue)
        {
             id = (int)routeId.Value;
        }
        try
        {
            var route = await routeService.UpdateRoute(id, authData.ClientId, authData.UserId);
            if (route == null)
            {
                return NotFound();
            }
            return Ok(route);
        }
        catch (Exception ex)
        {
            return MapExceptionsToHttp(ex);
        }
    }

    [HttpDelete("{routeId}")]
    public async Task<IActionResult> CancelRoute(int routeId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            await routeService.CancelRoute(routeId, authData.ClientId, authData.UserId);
            return Ok($"Se ha cancelado la Ruta con código {routeId}");
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