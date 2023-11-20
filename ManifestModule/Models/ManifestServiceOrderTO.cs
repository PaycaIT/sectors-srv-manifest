namespace sectors_srv_manifest.ManifestModule.Models;

public class ManifestServiceOrderTO
{
    public int ClientOrdinal { get; set; }
    public string ExternalId { get; set; } = "";
    public string Client { get; set; } = "";
    public string RecipientAddress { get; set; } = "";
    public string RecipientName { get; set; } = "";


}
