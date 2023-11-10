using Dapper;
using Microsoft.Data.SqlClient;
using sectors_srv_manifest.Db;
using sectors_srv_manifest.ManifestModule.Exceptions;
using sectors_srv_manifest.ManifestModule.Models;
using sectors_srv_manifest.ManifestModule.Models.Reqs;
using System.Data;
using System.Text.Json;

namespace sectors_srv_manifest.ManifestModule.Dao;

public class ManifestDao
{
    public async Task<ManifestTO> CreateManifest(CreateManifestReqModel data, int clientId, string userId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        var errorCode = new SqlParameter("@ErrorCode", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        var errorDesc = new SqlParameter("@ErrorDesc", SqlDbType.NVarChar, 200)
        {
            Direction = ParameterDirection.Output
        };

        var parameters = new DynamicParameters();
        parameters.Add("@CourierId", data.CourierId);
        parameters.Add("@SectorId", data.SectorId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@CreatedBy", userId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        ManifestTO? manifest = await connection.QuerySingleOrDefaultAsync<ManifestTO>("PrcCreateManifest", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || manifest == null)
        {
            throw new BadRequestException(errDesc);
        }

        return manifest;
    }

    public async Task<ManifestTO> GetSingleManifest(int id, int clientId)
    {
        string sql = @"
	        SELECT TOP 1
	            Manifest.Id,
	            Manifest.CreatedAt,
	            Manifest.CreatedBy,
                Manifest.Status,
	            Courier.Id as 'Courier.Id',
	            Courier.Name as 'Courier.Name',
	            Courier.PersonalId as 'Courier.PersonalId',
	            Courier.VehiclePlate as 'Courier.VehiclePlate',
	            Courier.CreatedAt as 'Courier.CreatedAt',
                Sector.Id as 'Sector.Id',
                Sector.Code as 'Sector.Code',
	            Client.Id as 'ClientId'
	        FROM Manifest
	        LEFT JOIN Courier ON Manifest.CourierId = Courier.Id
	        INNER JOIN Client ON Client.Id = Manifest.ClientId
            INNER JOIN Sector ON Manifest.SectorId = Sector.Id
	        WHERE Manifest.Id = @Id and Manifest.ClientId = @ClientId
	        ;";

        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();
        ManifestTO? manifest = await connection.QuerySingleOrDefaultAsync<ManifestTO>(sql, new { Id = id, ClientId = clientId });

        if (manifest == null)
        {
            throw new EntityNotFoundException("Manifiesto no existe");
        }

        return manifest;
    }

    public async Task<(IEnumerable<ManifestTO>, int)> GetManyManifest(ManifestFiltersReqModel filters, int clientId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@CreatedByFrom", filters.CreatedByFrom);
        parameters.Add("@CreatedByTo", filters.CreatedByTo);
        parameters.Add("@Status", filters.Status);
        parameters.Add("@PageNumber", filters.PageNumber);
        parameters.Add("@PageSize", filters.PageSize);
        parameters.Add("@SortColumn", filters.SortColumn);
        parameters.Add("@SortOrder", filters.SortOrder);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

        string prc = "PrcGetManifests";

        IEnumerable<ManifestTO> manifests = await connection.QueryAsync<ManifestTO>(prc, parameters, commandType: CommandType.StoredProcedure);

        if (manifests == null || !manifests.Any())
        {
            throw new ArgumentException("Error al obtener los manifiestos");
        }

        int totalCount = parameters.Get<int>("@TotalCount");

        return (manifests, totalCount);
    }

    public async Task<ManifestTO?> UpdateManifest(UpdateManifestReqModel data, int clientId, string userId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", data.Id);
        parameters.Add("@CourierId", data.CourierId);
        parameters.Add("@SectorId", data.SectorId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@UpdatedBy", userId);
        parameters.Add("@Status", data.Status);
        parameters.Add("@UpdatedRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        ManifestTO? manifest = await connection.QuerySingleOrDefaultAsync<ManifestTO>("PrcUpdateManifest", parameters, commandType: CommandType.StoredProcedure);

        int affectedRows = parameters.Get<int>("@UpdatedRows");
        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (affectedRows == 0)
        {
            throw new EntityNotFoundException("Manifiesto no existe");
        }

        if (errCode != 0 || manifest == null)
        {
            throw new ArgumentException(errDesc);
        }

        return manifest;
    }

    public async Task SoftDeleteManifest(int Id, int clientId, string userId)
    {
        string sql = @"
            UPDATE Manifest
            SET SoftDeleted = 1
            Where Id = @Id and ClientId = @ClientId;
        ";

        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();
        int rowsAffected = await connection.ExecuteAsync(sql, new { Id = Id, ClientId = clientId });

        if (rowsAffected == 0)
        {
            throw new EntityNotFoundException("Manifiesto no existe");
        }
    }

    public async Task<ManifestTO?> SmartAssociate(string serviceOrderExternalId, int sectorId, int clientId, string userId, bool force = false)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        // Initialize parameters with default values and output parameters
        var parameters = new DynamicParameters();
        parameters.Add("@ServiceOrderExternalId", serviceOrderExternalId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@SectorId", sectorId);
        parameters.Add("@CreatedBy", userId);
        parameters.Add("@Force", force);
        parameters.Add("@FoundManifestClientOrdinal", dbType: DbType.Int32, direction: ParameterDirection.Output); //Si la OS estaba asociada a un manifesto lo tendremos aqui, aunque no se haya forzado la asociacion 
        parameters.Add("@FoundManifestSectorCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

        // Execute the stored procedure within a transaction
        ManifestTO? manifest = await connection.QuerySingleOrDefaultAsync<ManifestTO>("dbo.PrcAddServiceOrderToManifest", parameters, commandType: CommandType.StoredProcedure);

        // Read the output values
        int errorCode = parameters.Get<int>("@ErrorCode");
        string errorDesc = parameters.Get<string>("@ErrorDesc");

        // Optionally, check errorCode and decide whether to commit
        if (errorCode != 0 || manifest == null)
        {
            if (errorCode == 1) // OS no existe
            {
                throw new EntityNotFoundException(errorDesc);
            }
            else if (errorCode == 2) // SectorTO no existe
            {
                throw new EntityNotFoundException(errorDesc);
            }
            else if (errorCode == 3) //OS ya asociada al mismo manifiesto abierto
            {
                throw new OSAlreadyInManifestException(errorDesc);
            }
            else if (errorCode == 4) //OS ya asociada a otro manifiesto
            {
                int foundManifestClientOrdinal = parameters.Get<int>("@FoundManifestClientOrdinal");
                string foundManifestSectorCode = parameters.Get<string>("@FoundManifestSectorCode");
                throw new OSAlreadyInManifestException(errorDesc, foundManifestClientOrdinal.ToString(), foundManifestSectorCode);
            }
            throw new Exception(errorDesc);
        }
        return manifest;
    }

    public async Task<IEnumerable<ManifestServiceOrderTO>> ListServiceOrders(int manifestId, int clientId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ManifestId", manifestId);
        parameters.Add("@ClientId", clientId);
        using (SqlConnection connection = ConnectionFactory.GetConnection())
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<ManifestServiceOrderTO>("PrcGetManifestServiceOrders", parameters, commandType: CommandType.StoredProcedure);
            if (result == null || result.Count() == 0)
            {
                throw new EntityNotFoundException("Manifiesto no existe");
            }

            return result;
        }

    }

    //public async Task<Manifest> AddServiceOrderToManifest()
    //{

    //}

    //public async Task<Manifest> RemoveServiceOrderFromManifest()
    //{

    //}

    //public async Task<Manifest> GetManifestByServiceOrder()
    //{

    //}
}
