using Dapper;
using Microsoft.Data.SqlClient;
using sectors_srv_manifest.Db;
using sectors_srv_manifest.RouteModule.Exceptions;
using System.Data;
using System.Text.Json;

namespace sectors_srv_manifest.RouteModule.Dao;

public class RouteDao
{
    public async Task<Route?> CreateRoute(string startingManifestId, int courierId, int clientId, string userId)
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
        parameters.Add("@StartingManifestId", startingManifestId);
        parameters.Add("@CourierId", courierId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@CreatedBy", userId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        string? jsonResult = await connection.QuerySingleOrDefaultAsync<string>("PrcCreateRoute", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || jsonResult == null)
        {
            throw new BadRequestException(errDesc);
        }

        Route? route = JsonSerializer.Deserialize<Route?>(jsonResult);

        if (route == null)
        {
            throw new ArgumentException("Error al crear la ruta");
        }

        return route;
    }
}
