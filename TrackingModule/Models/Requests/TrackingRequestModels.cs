namespace sectors_srv_manifest.TrackingModule.Models.Reqs;

public class CreateSOTrackingReq
{
    public int? ServiceOrderId { get; set; }
    public string EvExCode { get; set; } = "DR";
}


public class UpdateSOTrackingReq : CreateSOTrackingReq
{
    public int Id { get; set; }
}
