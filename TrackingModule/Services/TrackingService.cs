﻿using sectors_srv_manifest.TrackingModule.Models.Reqs;
using sectors_srv_manifest.TrackingModule.Models;
using sectors_srv_manifest.TrackingModule.Dao;
using sectors_srv_courier.CourierModule.Dao;

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

    public async Task<(IEnumerable<SOTrackingTO>, int)> GetSOTrackings(TrackingFiltersReq filters, int clientId)
    {
        if (filters == null)
        {
            throw new ArgumentException("Filtros son requeridos");
        }
        return await trackingDao.GetSOTrackings(filters, clientId);
    }


}