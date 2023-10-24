namespace sectors_srv_manifest.Configuration;

public class ConfigurationManager
{
    private static readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    public static readonly string DbConnectionString = configuration.GetConnectionString("connectionString");
}
