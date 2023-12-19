namespace sectors_srv_manifest.TrackingModule.Models.Reqs;

public class CreateSOTrackingReq
{
    public int? ServiceOrderId { get; set; }
    public string EvExCode { get; set; } = "ASSIGNED";
}


public class UpdateSOTrackingReq : CreateSOTrackingReq
{
    public int Id { get; set; }
}

public class CreateDeliveryData
{
    public int TrackingId { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPersonalId { get; set; } = string.Empty;
    public string RecipientSignature { get; set; } = string.Empty;

}

public class TriggerEventReq
{
    public string EvExCode { get; set; } = string.Empty;
    public int ServiceOrderId { get; set; }
    public string? Image { get; set; } = string.Empty;
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
    public string? RecipientName { get; set; } = string.Empty;
    public string? RecipientPersonalId { get; set; } = string.Empty;
    public string? RecipientSignature { get; set; } = string.Empty;

}

public class TriggerNobodyHomeEventReq
{
    public int ServiceOrderId { get; set; }
    public string? Image { get; set; } = string.Empty;
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }

}

public class TriggerDeliveredEventReq : TriggerNobodyHomeEventReq
{
    public string? RecipientName { get; set; } = string.Empty;
    public string? RecipientPersonalId { get; set; } = string.Empty;
    public string? RecipientSignature { get; set; } = string.Empty;

}
