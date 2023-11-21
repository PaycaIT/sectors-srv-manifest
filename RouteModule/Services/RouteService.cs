using sectors_srv_manifest.ManifestModule.Models;
using sectors_srv_manifest.RouteModule.Dao;
using sectors_srv_manifest.RouteModule.Models;
using sectors_srv_manifest.RouteModule.Models.Reqs;
using sectors_srv_manifest.TrackingModule.Dao;
using sectors_srv_manifest.TrackingModule.Models;
using System.Collections.Generic;

namespace sectors_srv_manifest.RouteModule.Services;

public class RouteService
{
    private readonly RouteDao routeDao = new();
    private readonly TrackingDao trackingDao = new();

    public async Task<RouteTO?> CreateRoute(CreateRouteReq data, int clientId, string userId)
    {
        if (data.StartingManifestId == null)
        {
            throw new ArgumentException("Se requiere una Ruta valida");
        }
        RouteTO? route = await routeDao.CreateRoute(data, clientId, userId);

        if (route == null)
        {
            throw new ArgumentException("No se pudo crear la ruta");

        }

        RouteDetailTO? routeAssigned = await routeDao.AssignSOToRoute(data, route.Id,  clientId, userId);

        if (routeAssigned == null)
        {
            throw new ArgumentException("No se pudieron asignar OS a la nueva ruta");

        }
        IEnumerable<SOTrackingTO?> createdTracking = await trackingDao.CreateSOTrackingFromRoute(route.Id, clientId, userId);

        if (!createdTracking.Any())
        {
            throw new ArgumentException("Se creo la ruta pero no los trackings");

        }
        return route;
    }

    public async Task<RouteTO?> GetSingleRoute(int routeId, int clientId)
    {
        if (routeId <= 0)
        {
            throw new ArgumentException("ID de ruta inválido");
        }
        return await routeDao.GetSingleRoute(routeId, clientId);
    }

    public async Task<PaginatedResponse<RouteTO>> GetManyRoutes(RouteFiltersReq filters, int clientId)
    {
        if (filters == null)
        {
            throw new ArgumentException("Filtros son requeridos");
        }
        var (items, totalCount) = await routeDao.GetManyRoutes(filters, clientId);
        return new PaginatedResponse<RouteTO>
        {
            Items = items,
            Total = totalCount,
            PageNumber = filters.PageNumber,
            PageSize = filters.PageSize
        };
    }

    public async Task<RouteTO?> UpdateRoute(int routeId, int clientId, string userId)
    {
        if (routeId == 0)
        {
            throw new ArgumentException("routeId es requerido");
        }
        return await routeDao.UpdateRoute(routeId, clientId, userId);
    }

    public async Task CancelRoute(int Id, int clientId, string userId)
    {
        if (Id <= 0)
        {
            throw new ArgumentException("ID de ruta inválido");
        }
        await routeDao.CancelRoute(Id, clientId, userId);
    }

    public async Task<RouteDetailTO?> GetRouteDetail(int routeId, int clientId)
    {
        if (routeId <= 0)
        {
            throw new ArgumentException("Se requiere un route Id");
        }
        return await routeDao.GetRouteDetail(routeId, clientId);
    }

    public async Task<RouteDetailsReq?> GetRouteDetails(int routeId, int clientId)
    {
        if(routeId <= 0)
        {
            throw new ArgumentException("Se requiere un route Id");
        }
        return await routeDao.GetRouteDetails(routeId, clientId);
    }

}