using sectors_srv_manifest.TrackingModule.Models;
using System.Text.Json.Serialization;

namespace sectors_srv_manifest.RouteModule.Models.Reqs;

public class CreateRouteReq
{
    public int? StartingManifestId { get; set; }
    public int? CourierId { get; set; }
    public string Status { get; set; } = "IN_PROGRESS";
}


public class UpdateRouteReq : CreateRouteReq
{
    public int Id { get; set; }
}

public class RouteDetailsReq
{
    public int Id { get; set; }
    public RouteTO? Route { get; set; }
    public IEnumerable<ServiceOrderTO>? ServiceOrders { get; set;}
    public DateTime CreatedAt { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }  
}
