namespace sectors_srv_manifest.TrackingModule.Models.Res;



public class TriggerNobodyHomeEventRes
{
    public SOTrackingTO? Event { get; set; } = new();
    public ServiceOrderTrackingDetailTO? Detail { get; set; } = new();


}

public class TriggerDeliveredEventRes : TriggerNobodyHomeEventRes
{
    public DeliveryDataTO? RecipientData { get; set; } = new();


}
