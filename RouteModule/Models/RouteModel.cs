using System.Text.Json.Serialization;
namespace sectors_srv_manifest.RouteModule.Models;

public class RouteModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public int StartingManifestId { get; set; }
    public int CourierId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public int ClientId { get; set; }

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


