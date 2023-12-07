using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sectors_service_orders.Auth;
using sectors_srv_manifest.AuthModule.Exceptions;
using sectors_srv_manifest.AuthModule.Models;
using sectors_srv_manifest.RouteModule.Exceptions;
using sectors_srv_manifest.RouteModule.Models;
using sectors_srv_manifest.RouteModule.Models.Reqs;
using sectors_srv_manifest.RouteModule.Services;
using sectors_srv_manifest.TrackingModule.Models;

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
    public async Task<IActionResult> CreateRoute(CreateRouteReq data)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        int routeId;
        try
        {
            var route = await routeService.CreateRoute(data, authData.ClientId, authData.UserId);
            routeId = route.Id;

            if (route != null)
            {
                var responseData = new CreateRouteResponse
                {
                    Route = route,
                    AssignedRoutes = new List<RouteDetailTO>(),
                    CreatedTrackings = new List<SOTrackingTO>()
                };

                // Asignar OS a la nueva ruta
                foreach (var manifestId in data.ManifestIds)
                {
                    var assignedRoutes = await routeService.AssignSOToRoute(manifestId, route.Id, authData.ClientId, authData.UserId);
                    if (assignedRoutes == null || !assignedRoutes.Any())
                    {
                        return BadRequest("No se pudieron asignar OS a la nueva ruta");
                    }

                    responseData.AssignedRoutes.AddRange(assignedRoutes);
                }

                // Crear trackings a partir de la nueva ruta
                var createdTrackings = await routeService.CreateSOTrackingsFromRoute(route.Id, authData.ClientId, authData.UserId);

                if (createdTrackings == null || !createdTrackings.Any())
                {
                    return BadRequest("Se creo la ruta pero no los trackings");
                }

                responseData.CreatedTrackings.AddRange(createdTrackings);

                return Ok(responseData);
            }

            return BadRequest("No se pudo crear la ruta");
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

    [HttpGet("{routeId}/detail")]
    public async Task<IActionResult> GetRouteDetail(int routeId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var route = await routeService.GetRouteDetail(routeId, authData.ClientId);
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

    [HttpGet("{routeId}/details")]
    public async Task<IActionResult> GetRouteDetails(int routeId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var route = await routeService.GetRouteDetails(routeId, authData.ClientId);
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

    [HttpGet("courier-data/{courierId}")]
    public async Task<IActionResult> GetDetailedRoutesData(int courierId)
    {
        JwtModel authData = JWTUtils.GetAuthData(User.Claims);
        try
        {
            var routes = await routeService.GetDetailedRoutesData(courierId, authData.ClientId);
            if (routes == null)
            {
                return NotFound();
            }
            return Ok(routes);
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
            var paginatedResponse = await routeService.GetManyRoutes(filters, authData.ClientId);
            return Ok(paginatedResponse);
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