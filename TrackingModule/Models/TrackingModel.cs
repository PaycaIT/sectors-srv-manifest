using System.Text.Json.Serialization;

namespace sectors_srv_manifest.TrackingModule.Models;

public class SOTrackingTO
{
    public int Id { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    public int ServiceOrderId { get; set; }
    public string SOExternalId { get; set; } = string.Empty;
    public string EvExCode { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool Synced { get; set; } = true;
}

public class EventsErrorTO
{
    public int Id { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class ServiceOrderTO
{
    public int? Id { get; set; }
    public string? ExternalId { get; set; }
    public int DispatcherId { get; set; }
    public int RecipientId { get; set; }
    public int DispatcherDirId { get; set; }
    public int RecipientDirId { get; set; }
    public int DimensionId { get; set; }
    public int ValueId { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    public int? BatchId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
    public string? EvExCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public DateTime SoftDeleted { get; set; }
}

public class ServiceOrderTrackingDetailTO
{
    public int Id { get; set; }
    public int TrackingId { get; set; }
    public string? Image { get; set; } = string.Empty;
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }

}

public class DeliveryDataTO
{
    public int Id { get; set; }
    public int TrackingId { get; set; }
    public string? RecipientName { get; set; } = string.Empty;
    public string? RecipientPersonalId { get; set; } = string.Empty;
    public string? RecipientSignature { get; set; } = string.Empty;

}