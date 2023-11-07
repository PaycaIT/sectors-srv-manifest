using sectors_srv_manifest.ManifestModule.Dao;
using sectors_srv_manifest.ManifestModule.Models;
using sectors_srv_manifest.ManifestModule.Models.Reqs;

namespace sectors_srv_manifest.ManifestModule.Services;

public class ManifestService
{
    private readonly ManifestDao manifestDao = new ManifestDao();

    public async Task<ManifestModel> CreateManifest(CreateManifestReqModel data, int cliendId, string userId)
    {
        if (data == null || data.SectorId == null)
        {
            throw new ArgumentException("SectorId es requerido");
        }
        return await manifestDao.CreateManifest(data, cliendId, userId);
    }

    public async Task<ManifestModel> GetSingleManifest(int id, int clientId)
    {
        return await manifestDao.GetSingleManifest(id, clientId);
    }

    public async Task<PaginatedResponse<ManifestModel>> GetManyManifest(ManifestFiltersReqModel filters, int clientId)
    {
        var (items, totalCount) = await manifestDao.GetManyManifest(filters, clientId);
        return new PaginatedResponse<ManifestModel>
        {
            Items = items,
            Total = totalCount,
            PageNumber = filters.PageNumber,
            PageSize = filters.PageSize
        };
    }

    public async Task<ManifestModel> UpdateManifest(UpdateManifestReqModel data, int clientId, string userId)
    {
        return await manifestDao.UpdateManifest(data, clientId, userId);
    }

    public async Task SoftDeleteManifest(int id, int clientId, string userId)
    {
        await manifestDao.SoftDeleteManifest(id, clientId, userId);
    }

    public async Task<ManifestModel> SmartAssociate(string serviceOrderExternalId, int sectorId, int clientId, string userId, bool force = false)
    {
        var a = await manifestDao.SmartAssociate(serviceOrderExternalId, sectorId, clientId, userId, force);
        return a;
    }

    //public async Task<Manifest> AddServiceOrderToManifest()
    //{

    //}

    //public async Task RemoveServiceOrderFromManifest()
    //{

    //}
}
