using sectors_srv_courier.CourierModule.Dao;
using sectors_srv_manifest.CourierModule.Models;
using sectors_srv_manifest.CourierModule.Models.Reqs;

namespace sectors_srv_manifest.CourierModule.Services;

public class CourierService
{
    private readonly CourierDao courierDao = new();

    public async Task<CourierTO?> CreateCourier(CreateCourierReq data, int cliendId, string userId)
    {
        if (data == null)
        {
            throw new ArgumentException("Data es requerido");
        }
        return await courierDao.CreateCourier(data, cliendId, userId);
    }

    public async Task<CourierTO?> GetSingleCourier(int courierId, int clientId)
    {
        if (courierId <= 0)
        {
            throw new ArgumentException("ID de courier inválido");
        }
        return await courierDao.GetSingleCourier(courierId, clientId);
    }

    public async Task<(IEnumerable<CourierTO>, int)> GetManyCouriers(CourierFiltersReq filters, int clientId)
    {
        if (filters == null)
        {
            throw new ArgumentException("Filtros son requeridos");
        }
        return await courierDao.GetManyCouriers(filters, clientId);
    }

    public async Task<CourierTO?> UpdateCourier(UpdateCourierReq data, int clientId, string userId)
    {
        if (data == null)
        {
            throw new ArgumentException("Data es requerido");
        }
        return await courierDao.UpdateCourier(data, clientId, userId);
    }

    public async Task SoftDeleteCourier(int Id, int clientId, string userId)
    {
        if (Id <= 0)
        {
            throw new ArgumentException("ID de courier inválido");
        }
        await courierDao.SoftDeleteCourier(Id, clientId, userId);
    }
}
