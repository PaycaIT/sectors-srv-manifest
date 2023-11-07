using Dapper;
using Microsoft.Data.SqlClient;
using sectors_srv_manifest.Db;
using sectors_srv_manifest.RouteModule.Exceptions;
using sectors_srv_manifest.RouteModule.Models.Reqs;
using System.Data;
using System.Text.Json;

namespace sectors_srv_manifest.RouteModule.Dao;

public class RouteDao
{
    public async Task<Route?> CreateRoute(CreateRouteReq data, int clientId, string userId)
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
        parameters.Add("@StartingManifestId", data.StartingManifestId);
        parameters.Add("@CourierId", data.CourierId);
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

        Route? route = JsonSerializer.Deserialize<Route>(jsonResult);

        if (route == null)
        {
            throw new ArgumentException("Error al crear la ruta");
        }

        return route;
    }

    public async Task<Route> GetSingleRoute(int routeId, int clientId)
    {
        string sql = @"
        SELECT
            Route.Id,
            Route.StartingManifestId,
            Route.CourierId,
            Route.CreatedAt,
            Route.CreatedBy,
            Route.Status
        FROM Route
        WHERE Route.Id = @RouteId AND ClientId = @ClientId;";

        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();
        var route = await connection.QuerySingleOrDefaultAsync<Route>(sql, new { RouteId = routeId, ClientId = clientId });

        if (route == null)
        {
            throw new EntityNotFoundException("Ruta no encontrada");
        }

        return route;
    }

    public async Task<(IEnumerable<Route>, int)> GetManyRoutes(RouteFiltersReq filters, int clientId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@StartingManifestId", filters.StartingManifestId);
        parameters.Add("@CourierId", filters.CourierId);
        parameters.Add("@PageNumber", filters.PageNumber);
        parameters.Add("@PageSize", filters.PageSize);
        parameters.Add("@SortColumn", filters.SortColumn);
        parameters.Add("@SortOrder", filters.SortOrder);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

        string prc = "PrcGetRoutes";

        var resultJson = await connection.QueryAsync<string>(prc, parameters, commandType: CommandType.StoredProcedure);

        if (resultJson == null || resultJson.Count() == 0)
        {
            throw new ArgumentException("Error al obtener las rutas");
        }

        int totalCount = parameters.Get<int>("@TotalCount");
        IEnumerable<Route> routes = JsonSerializer.Deserialize<IEnumerable<Route>>(String.Join("", resultJson));

        if (routes == null)
        {
            throw new ArgumentException("Error al obtener las rutas");
        }

        return (routes, totalCount);
    }

    public async Task<Route?> UpdateRoute(int routeId, int clientId, string userId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@RouteId", routeId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@UpdatedBy", userId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        string? jsonResult = await connection.QuerySingleOrDefaultAsync<string>("PrcUpdateRoute", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || jsonResult == null)
        {
            throw new ArgumentException(errDesc);
        }

        Route? route = JsonSerializer.Deserialize<Route>(jsonResult);

        if (route == null)
        {
            throw new ArgumentException("Error al actualizar la ruta");
        }

        return route;
    }

    public async Task<Route?> CancelRoute(int Id, int clientId, string userId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@RouteId", Id);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@UpdatedBy", userId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        string? jsonResult = await connection.QuerySingleOrDefaultAsync<string>("PrcCancelRoute", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || jsonResult == null)
        {
            throw new ArgumentException(errDesc);
        }

        Route? route = JsonSerializer.Deserialize<Route>(jsonResult);

        if (route == null)
        {
            throw new ArgumentException("Error al cancelar la ruta");
        }

        return route;
    }
}
