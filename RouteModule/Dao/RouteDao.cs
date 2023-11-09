using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using sectors_srv_manifest.Db;
using sectors_srv_manifest.RouteModule.Exceptions;
using sectors_srv_manifest.RouteModule.Models;
using sectors_srv_manifest.RouteModule.Models.Reqs;
using System.Data;

namespace sectors_srv_manifest.RouteModule.Dao;

public class RouteDao
{
    public async Task<RouteModel?> CreateRoute(CreateRouteReq data, int clientId, string userId)
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

        RouteModel? route = await connection.QuerySingleOrDefaultAsync<RouteModel>("PrcCreateRoute", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || route == null)
        {
            throw new BadRequestException(errDesc);
        }

        return route;
    }

    public async Task<RouteModel?> GetSingleRoute(int routeId, int clientId)
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
        RouteModel? route = await connection.QuerySingleOrDefaultAsync<RouteModel>(sql, new { RouteId = routeId, ClientId = clientId });

        if (route == null)
        {
            throw new EntityNotFoundException("Ruta no encontrada");
        }

        return route;
    }

    public async Task<(IEnumerable<RouteModel>, int)> GetManyRoutes(RouteFiltersReq filters, int clientId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@StartingManifestId", filters.StartingManifestId);
        parameters.Add("@CourierId", filters.CourierId);
        parameters.Add("@Status", filters.Status);
        parameters.Add("@PageNumber", filters.PageNumber);
        parameters.Add("@PageSize", filters.PageSize);
        parameters.Add("@SortColumn", filters.SortColumn);
        parameters.Add("@SortOrder", filters.SortOrder);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

        string prc = "PrcGetRoutes";

        IEnumerable<RouteModel> routes = await connection.QueryAsync<RouteModel>(prc, parameters, commandType: CommandType.StoredProcedure);

        if (routes == null || !routes.Any())
        {
            throw new ArgumentException("Error al obtener las rutas");
        }

        int totalCount = parameters.Get<int>("@TotalCount");

        return (routes, totalCount);
    }

    public async Task<RouteDetail?> GetRouteDetail(int routeId, int clientId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@RouteId", routeId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        string prc = "PrcGetRouteDetail";

        RouteDetail? routeDetails = await connection.QuerySingleOrDefaultAsync<RouteDetail>(prc, parameters, commandType: CommandType.StoredProcedure);

        int errorCode = parameters.Get<int>("@ErrorCode");
        string errorDesc = parameters.Get<string>("@ErrorDesc");

        if (routeDetails == null)
        {
            throw new ArgumentException("Error al obtener los detalles de la ruta");
        }

        return routeDetails;
    }

    public async Task<RouteDetailsReq?> GetRouteDetails(int routeId, int clientId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@RouteId", routeId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        string prc = "PrcGetRouteDetails";

        RouteDetailsReq? routeDetails = await connection.QuerySingleOrDefaultAsync<RouteDetailsReq>(prc, parameters, commandType: CommandType.StoredProcedure);

        int errorCode = parameters.Get<int>("@ErrorCode");
        string errorDesc = parameters.Get<string>("@ErrorDesc");

        if (routeDetails == null)
        {
            throw new ArgumentException("Error al obtener los detalles de la ruta");
        }

        return routeDetails;
    }

    public async Task<RouteModel?> UpdateRoute(int routeId, int clientId, string userId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@RouteId", routeId);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@UpdatedBy", userId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        RouteModel? route = await connection.QuerySingleOrDefaultAsync<RouteModel>("PrcUpdateRoute", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || route == null)
        {
            throw new ArgumentException(errDesc);
        }
        return route;
    }

    public async Task<RouteModel?> CancelRoute(int Id, int clientId, string userId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@RouteId", Id);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@UpdatedBy", userId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        RouteModel? route = await connection.QuerySingleOrDefaultAsync<RouteModel>("PrcCancelRoute", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || route == null)
        {
            throw new ArgumentException(errDesc);
        }

        return route;
    }
}
