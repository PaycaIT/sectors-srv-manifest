using Dapper;
using Microsoft.Data.SqlClient;
using sectors_srv_manifest.CourierModule.Exceptions;
using sectors_srv_manifest.CourierModule.Models;
using sectors_srv_manifest.CourierModule.Models.Reqs;
using sectors_srv_manifest.Db;
using System.Data;
using System.Text.Json;


namespace sectors_srv_courier.CourierModule.Dao;

public class CourierDao
{
    public async Task<Courier?> CreateCourier(CreateCourierReq data, int clientId, string userId)
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
        parameters.Add("@Name", data.Name);
        parameters.Add("@PersonalId", data.PersonalId);
        parameters.Add("@VehiclePlate", data.VehiclePlate);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@CreatedBy", userId);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        Courier? courier = await connection.QuerySingleOrDefaultAsync<Courier>("PrcCreateCourier", parameters, commandType: CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || courier == null)
        {
            throw new BadRequestException(errDesc);
        }

        return courier;
    }

    public async Task<Courier> GetSingleCourier(int courierId, int clientId)
    {
        string sql = @"
        SELECT
            Courier.Id,
            Courier.Name,
            Courier.PersonalId,
            Courier.VehiclePlate,
            Courier.CreatedAt,
            Courier.CreatedBy
        FROM Courier
        WHERE Courier.Id = @CourierId AND ClientId = @ClientId;";

        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();
        var courier = await connection.QuerySingleOrDefaultAsync<Courier>(sql, new { CourierId = courierId, ClientId = clientId });

        if (courier == null)
        {
            throw new EntityNotFoundException("Courier no encontrado");
        }

        return courier;
    }

    public async Task<(IEnumerable<Courier>, int)> GetManyCouriers(CourierFiltersReq filters, int clientId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@Name", filters.Name);
        parameters.Add("@PersonalId", filters.PersonalId);
        parameters.Add("@VehiclePlate", filters.VehiclePlate);
        parameters.Add("@PageNumber", filters.PageNumber);
        parameters.Add("@PageSize", filters.PageSize);
        parameters.Add("@SortColumn", filters.SortColumn);
        parameters.Add("@SortOrder", filters.SortOrder);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

        string prc = "PrcGetCouriers"; 

        IEnumerable<Courier> couriers = await connection.QueryAsync<Courier>(prc, parameters, commandType: CommandType.StoredProcedure);

        if (couriers == null || !couriers.Any())
        {
            throw new ArgumentException("Error al obtener los couriers");
        }

        int totalCount = parameters.Get<int>("@TotalCount");

        return (couriers, totalCount);
    }

    public async Task<Courier?> UpdateCourier(UpdateCourierReq data, int clientId, string userId)
    {
        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", data.Id);
        parameters.Add("@Name", data.Name);
        parameters.Add("@PersonalId", data.PersonalId);
        parameters.Add("@VehiclePlate", data.VehiclePlate);
        parameters.Add("@ClientId", clientId);
        parameters.Add("@UpdatedBy", userId);
        parameters.Add("@UpdatedRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ErrorDesc", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        Courier? courier = await connection.QuerySingleOrDefaultAsync<Courier>("PrcUpdateCourier", parameters, commandType: CommandType.StoredProcedure);

        int affectedRows = parameters.Get<int>("@UpdatedRows");
        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (affectedRows == 0)
        {
            throw new EntityNotFoundException("Courier no existe");
        }

        if (errCode != 0 || courier == null)
        {
            throw new ArgumentException(errDesc);
        }

        return courier;
    }

    public async Task SoftDeleteCourier(int Id, int clientId, string userId)
    {
        string sql = @"
            UPDATE Courier
            SET SoftDeleted = 1
            WHERE Id = @Id AND ClientId = @ClientId;
        ";

        using SqlConnection connection = ConnectionFactory.GetConnection();
        await connection.OpenAsync();
        int rowsAffected = await connection.ExecuteAsync(sql, new { Id = Id, ClientId = clientId });

        if (rowsAffected == 0)
        {
            throw new EntityNotFoundException("Courier no existe");
        }
    }
}
