using sectors_srv_manifest.CourierModule.Models;
using sectors_srv_manifest.TrackingModule.Models;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace sectors_srv_manifest.ManifestModule.Models;

public class ManifestTO
{
    public int Id { get; set; }
    public SectorTO? Sector { get; set; }
    public CourierTO? Courier { get; set; }
    public string CreatedBy { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = "";
    public int ClientOrdinal { get; set; }
    public int OsCount { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    public int? SectorId { get; set; }
    public string? SectorCode { get; set; }
}

public class SectorTO
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
}

public enum ManifestStatusEnum
{
    [Description(nameof(Created))]
    Created,
    InProgress,
    Completed,
    Closed
}

public class ManifestDetail{
    public int Id { get; set; }
    public IEnumerable<ServiceOrderTO>? ServiceOrders { get; set; }
}
