using Microsoft.AspNetCore.Routing;
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
        if (data.ManifestIds == null)
        {
            throw new ArgumentException("Se requiere una Ruta valida");
        }
        RouteTO? route = await routeDao.CreateRoute(data, clientId, userId);

        if (route == null)
        {
            throw new ArgumentException("No se pudo crear la ruta");

        }
        return route;
    }

    public async Task<IEnumerable<RouteDetailTO?>> AssignSOToRoute(int manifestId, int routeId, int clientId, string userId)
    {
        if (manifestId == 0)
        {
            throw new ArgumentException("Se requieren manifiestos validos para asignar OS");
        }

        IEnumerable<RouteDetailTO?> AssignedRoutes = await routeDao.AssignSOToRoute(manifestId, routeId, clientId, userId);

        if (AssignedRoutes == null)
        {
            throw new ArgumentException("No se pudieron asignar OS a la nueva ruta");

        }

        return AssignedRoutes;
    }

    public async Task<IEnumerable<SOTrackingTO?>> CreateSOTrackingsFromRoute(int routeId, int clientId, string userId)
    {
        if (routeId == 0)
        {
            throw new ArgumentException("Se requiere una Ruta valida para generar trackings");
        }

        IEnumerable<SOTrackingTO?> createdTrackings = await trackingDao.CreateSOTrackingFromRoute(routeId, clientId, userId);

        if (!createdTrackings.Any())
        {
            throw new ArgumentException("Se creo la ruta pero no los trackings");

        }
        return createdTrackings;
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

    public async Task<IEnumerable<RouteDetailTO?>> GetRouteDetail(int routeId, int clientId)
    {
        if (routeId <= 0)
        {
            throw new ArgumentException("Se requiere un route Id");
        }
        return await routeDao.GetRouteDetail(routeId, clientId);
    }

    public async Task<IEnumerable<RouteServiceOrderTO>> GetRouteDetails(int routeId, int clientId)
    {
        if(routeId <= 0)
        {
            throw new ArgumentException("Se requiere un route Id");
        }
        return await routeDao.GetRouteDetails(routeId, clientId);
    }

    public async Task<IEnumerable<DetailedRoutesData>> GetDetailedRoutesData(int courierId, int clientId)
    {
        if (courierId <= 0)
        {
            throw new ArgumentException("Se requiere un courier Id válido");
        }
        return await routeDao.GetDetailedRoutesData(courierId, clientId);
    }

}