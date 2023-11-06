using sectors_srv_manifest.CourierModule.Dao;
using sectors_srv_manifest.CourierModule.Models;
using sectors_srv_manifest.CourierModule.Models.Reqs;

namespace sectors_srv_manifest.CourierModule.Services;

public class CourierService
{
    private readonly CourierDao courierDao = new();

    public async Task<Courier?> CreateCourier(CreateCourierReq data, int cliendId, string userId)
    {
        if (data == null)
        {
            throw new ArgumentException("Data es requerido");
        }
        return await courierDao.CreateCourier(data, cliendId, userId);
    }
}
