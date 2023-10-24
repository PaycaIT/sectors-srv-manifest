namespace sectors_srv_manifest.ManifestModule.Models.Reqs;

public class SmartAssociateManifestReq
{
    public string ServiceOrderExternalId { get; set; } = "";
    public int SectorId { get; set; }
    public bool Force { get; set; } = false;
}
