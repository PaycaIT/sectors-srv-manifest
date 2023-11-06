using System.Text.Json.Serialization;

namespace sectors_srv_manifest.CourierModule.Models;

public class Courier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PersonalId { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; } = string.Empty;
    [JsonIgnore]
    public int ClientId { get; set; }
}