namespace sectors_srv_manifest.Db;
using Microsoft.Data.SqlClient;
using sectors_srv_manifest.Configuration;

public static class ConnectionFactory
{
    private static readonly string DbConnectionString = ConfigurationManager.DbConnectionString;

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(DbConnectionString);
    }
}
