using sectors_srv_manifest.RouteModule.Dao;
using sectors_srv_manifest.RouteModule.Models.Reqs;

namespace sectors_srv_manifest.RouteModule.Services;

public class RouteService
{
    private readonly RouteDao routeDao = new();

    public async Task<Route?> CreateRoute(string startingManifestId, int courierId, int clientId, string userId)
    {
        if (startingManifestId == null)
        {
            throw new ArgumentException("Se requiere un Manifest Id es requerido");
        }
        return await routeDao.CreateRoute(startingManifestId, courierId, clientId, userId);
    }

    public async Task<Route?> GetSingleRoute(int routeId, int clientId)
    {
        if (routeId <= 0)
        {
            throw new ArgumentException("ID de ruta inválido");
        }
        return await routeDao.GetSingleRoute(routeId, clientId);
    }

    public async Task<(IEnumerable<Route>, int)> GetManyRoutes(RouteFiltersReq filters, int clientId)
    {
        if (filters == null)
        {
            throw new ArgumentException("Filtros son requeridos");
        }
        return await routeDao.GetManyRoutes(filters, clientId);
    }

    public async Task<Route?> UpdateRoute(UpdateRouteReq data, int clientId, string userId)
    {
        if (data == null)
        {
            throw new ArgumentException("Data es requerido");
        }
        return await routeDao.UpdateRoute(data, clientId, userId);
    }

    public async Task SoftDeleteRoute(int Id, int clientId, string userId)
    {
        if (Id <= 0)
        {
            throw new ArgumentException("ID de ruta inválido");
        }
        await routeDao.SoftDeleteRoute(Id, clientId, userId);
    }
}