namespace sectors_srv_manifest.Configuration;

public class CustomConfigurationManager
{
    public static readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    public static readonly string DbConnectionString = configuration.GetConnectionString("DefaultConnection");
}
