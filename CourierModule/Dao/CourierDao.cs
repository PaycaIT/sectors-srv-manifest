using Dapper;
using Microsoft.Data.SqlClient;
using sectors_srv_manifest.Db;
using sectors_srv_manifest.CourierModule.Exceptions;
using sectors_srv_manifest.CourierModule.Models;
using sectors_srv_manifest.CourierModule.Models.Reqs;
using System.Data;
using System.Text.Json;

namespace sectors_srv_manifest.CourierModule.Dao;

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

        string? jsonResult = await connection.QuerySingleOrDefaultAsync<string>("PrcCreateCourier", parameters, commandType: System.Data.CommandType.StoredProcedure);

        int errCode = parameters.Get<int>("@ErrorCode");
        string errDesc = parameters.Get<string>("@ErrorDesc");

        if (errCode != 0 || jsonResult == null)
        {
            throw new BadRequestException(errDesc);
        }

        Courier? courier = JsonSerializer.Deserialize<Courier?>(jsonResult);

        if (courier == null)
        {
            throw new ArgumentException("Error al crear el manifiesto");
        }

        return courier;
    }

}
