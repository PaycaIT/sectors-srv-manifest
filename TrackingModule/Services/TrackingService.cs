using sectors_srv_manifest.TrackingModule.Models.Reqs;
using sectors_srv_manifest.TrackingModule.Models;
using sectors_srv_manifest.TrackingModule.Dao;
using sectors_srv_courier.CourierModule.Dao;
using sectors_srv_manifest.ManifestModule.Models;

namespace sectors_srv_manifest.TrackingModule.Services;

public class TrackingService
{
    private readonly TrackingDao trackingDao = new();

    public async Task<SOTrackingTO?> CreateSOTracking(CreateSOTrackingReq data, int clientId, string userId)
    {
        if (data == null)
        {
            throw new ArgumentException("Data es requerido");
        }
        return await trackingDao.CreateSOTracking(data, clientId, userId);
    }
    public async Task<IEnumerable<SOTrackingTO?>> CreateSOTrackingFromRoute(int routeId, CreateSOTrackingReq data, int clientId, string userId) { 
        
        if(data == null || routeId == 0) 
        {
            throw new ArgumentException("Data es requerido");

        }
        return await trackingDao.CreateSOTrackingFromRoute(routeId, clientId, userId);

    }

    public async Task<PaginatedResponse<SOTrackingTO>> GetSOTrackings(TrackingFiltersReq filters, int clientId)
    {
        if (filters == null)
        {
            throw new ArgumentException("Filtros son requeridos");
        }
        var (items, totalCount) = await trackingDao.GetSOTrackings(filters, clientId);
        return new PaginatedResponse<SOTrackingTO>
        {
            Items = items,
            Total = totalCount,
            PageNumber = filters.PageNumber,
            PageSize = filters.PageSize
        };
    }


}
