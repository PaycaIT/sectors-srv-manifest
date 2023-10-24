namespace sectors_srv_manifest.ManifestModule.Models.Reqs;

public class CreateManifestReqModel
{
    public int? CourierId { get; set; }
    public int? SectorId { get; set; }
    public string Status { get; set; } = "OPEN";
}

public class UpdateManifestReqModel : CreateManifestReqModel
{
    public int Id { get; set; }
}