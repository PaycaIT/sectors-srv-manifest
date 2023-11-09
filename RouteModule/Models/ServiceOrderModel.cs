namespace sectors_srv_manifest.RouteModule.Models;

public class ServiceOrder
{
    public int? Id { get; set; }
    public string? ExternalId { get; set; }
    public int DispatcherId { get; set; }
    public int RecipientId { get; set; }
    public int DispatcherDirId { get; set; }
    public int RecipientDirId { get; set; }
    public int DimensionId { get; set; }
    public int ValueId { get; set; }
    public int ClientId { get; set; }
    public int? BatchId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public DateTime SoftDeleted { get; set; }
}

public class ServiceOrderTracking
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ServiceOrderId { get; set; }
    public int SOExternalId { get; set; }
    public string EvExCode { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
