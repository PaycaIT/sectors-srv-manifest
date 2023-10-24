using System.ComponentModel;
using System.Text.Json.Serialization;

namespace sectors_srv_manifest.ManifestModule.Models;

public class ManifestModel
{
    public int Id { get; set; }
    public Sector? Sector { get; set; }
    public CourierModel? Courier { get; set; }
    public string CreatedBy { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = "";
    public int ClientOrdinal { get; set; }
    public int OsCount { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
}

public class Sector
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
