using sectors_srv_manifest.CourierModule.Models;
using sectors_srv_manifest.ManifestModule.Models;
using System.Text.Json.Serialization;

namespace sectors_srv_manifest.RouteModule.Models.Reqs;

public class CreateRouteReq
{
    public ManifestModel? OriginManifest { get; set; }
    public ManifestModel? StartingManifest { get; set; }
    public Courier? Courier { get; set; }
    public string Status { get; set; } = "IN_PROGRESS";
}


public class UpdateRouteReq : CreateRouteReq
{
    public int Id { get; set; }
}

public class RouteDetailReq
{
    public int Id { get; set; }
    public ServiceOrder? ServiceOrder { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Route? Route { get; set; }
}
