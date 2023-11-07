using System.Text.Json.Serialization;
namespace sectors_srv_manifest.RouteModule.Models;

public class Route
{
    public int Id { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string StartingManifestId { get; set; } = string.Empty;
    public int CourierId { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class RouteDetail
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int ServiceOrderId { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    public DateTime CreatedAt { get; set; }
}


