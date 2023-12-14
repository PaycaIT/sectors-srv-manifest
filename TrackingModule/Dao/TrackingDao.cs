using Dapper;
using Microsoft.Data.SqlClient;
using sectors_srv_manifest.Db;
using sectors_srv_manifest.TrackingModule.Exceptions;
using sectors_srv_manifest.TrackingModule.Models;
using sectors_srv_manifest.TrackingModule.Models.Reqs;
using sectors_srv_manifest.TrackingModule.Models.Res;
using System.Data;

namespace sectors_srv_manifest.TrackingModule.Dao;

public class TrackingDao
{
    public async Task<SOTrackingTO?> CreateSOTracking(CreateSOTrackingReq data, int clientId, string userId)
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
        parameters.Add("@ServiceOrderId", data.ServiceOrderId);
        parameters.Add("@EvExCode", data.EvExCode);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        SOTrackingTO? soTracking = await connection.QuerySingleOrDefaultAsync<SOTrackingTO>("PrcCreateSOTracking", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || soTracking == null)
        {
            throw new BadRequestException(errDesc);
        }

        return soTracking;
    }

    public async Task<IEnumerable<SOTrackingTO?>> CreateSOTrackingFromRoute(int routeId, int clientId, string userId)
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
        parameters.Add("@RouteId", routeId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        IEnumerable<SOTrackingTO?> soTrackings = await connection.QueryAsync<SOTrackingTO>("PrcCreateSOTrackingRoute", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || soTrackings == null)
        {
            throw new BadRequestException(errDesc);
        }

        return soTrackings;
    }

    public async Task<(IEnumerable<SOTrackingTO>, int)> GetSOTrackings(TrackingFiltersReq filters, int clientId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@ClientId", clientId);
        parameters.Add("@ServiceOrderId", filters.ServiceOrderId);
        parameters.Add("@SOExternalId", filters.SOExternalId);
        parameters.Add("@EvExCode", filters.EvExCode);
        parameters.Add("@PageNumber", filters.PageNumber);
        parameters.Add("@PageSize", filters.PageSize);
        parameters.Add("@SortColumn", filters.SortColumn);
        parameters.Add("@SortOrder", filters.SortOrder);
        parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

        string prc = "PrcGetSOTrackings";

        IEnumerable<SOTrackingTO> soTrackings = await connection.QueryAsync<SOTrackingTO>(prc, parameters, commandType: CommandType.StoredProcedure);

        if (soTrackings == null || !soTrackings.Any())
        {
            throw new ArgumentException("Error al obtener los detalles de ServiceOrderTracking");
        }

        int totalCount = parameters.Get<int>("@TotalCount");

        return (soTrackings, totalCount);
    }

    public async Task<TriggerNobodyHomeEventRes?> TriggerNobodyHomeEvent(TriggerNobodyHomeEventReq data, int clientId, string userId)
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
        parameters.Add("@ServiceOrderId", data.ServiceOrderId);
        parameters.Add("@Image", data.Image);
        parameters.Add("@Latitude", data.Latitude);
        parameters.Add("@Longitude", data.Longitude);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        var detail = await connection.QuerySingleOrDefaultAsync<TriggerNobodyHomeEventRes>("PrcTriggerNobodyHomeEvent", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || detail == null)
        {
            throw new BadRequestException(errDesc);
        }

        return detail;
    }

    public async Task<TriggerDeliveredEventRes?> TriggerDeliveredEvent(TriggerDeliveredEventReq data, int clientId, string userId)
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
        parameters.Add("@ServiceOrderId", data.ServiceOrderId);
        parameters.Add("@Image", data.Image);
        parameters.Add("@Latitude", data.Latitude);
        parameters.Add("@Longitude", data.Longitude);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@RecipientName", data.RecipientName);
        parameters.Add("@RecipientPersonalId", data.RecipientPersonalId);
        parameters.Add("@RecipientSignature", data.RecipientSignature);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        var detail = await connection.QuerySingleOrDefaultAsync<TriggerDeliveredEventRes>("PrcTriggerDeliveredEvent", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || detail == null)
        {
            throw new BadRequestException(errDesc);
        }

        return detail;
    }
}
