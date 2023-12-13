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
