namespace sectors_srv_manifest.RouteModule.Models;

public class RouteServiceOrderTO
{
    public int Id { get; set; }
    public int ServiceOrderId { get; set; }
    public string ExternalId { get; set; } = "";
    public string Client { get; set; } = "";
    public string RecipientAddress { get; set; } = "";
    public string RecipientName { get; set; } = "";
    public string RecipientPhone { get; set; } = "";
    public string ReferenceDir { get; set; } = "Por implementar";
}
