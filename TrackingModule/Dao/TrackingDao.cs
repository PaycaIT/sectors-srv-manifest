using Dapper;
using Microsoft.Data.SqlClient;
using sectors_srv_manifest.Db;
using sectors_srv_manifest.TrackingModule.Exceptions;
using sectors_srv_manifest.TrackingModule.Models;
using sectors_srv_manifest.TrackingModule.Models.Reqs;
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
}
