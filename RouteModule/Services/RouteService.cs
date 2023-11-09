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
        IEnumerable<SOTrackingTO?> createdTracking = await trackingDao.CreateSOTrackingFromRoute(route.Id, clientId, userId);

        if (createdTracking == null)
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

    public async Task<(IEnumerable<RouteTO>, int)> GetManyRoutes(RouteFiltersReq filters, int clientId)
    {
        if (filters == null)
        {
            throw new ArgumentException("Filtros son requeridos");
        }
        return await routeDao.GetManyRoutes(filters, clientId);
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