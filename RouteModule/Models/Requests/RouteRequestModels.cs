using sectors_srv_manifest.TrackingModule.Models;
using System.Text.Json.Serialization;

namespace sectors_srv_manifest.RouteModule.Models.Reqs;

public class CreateRouteReq
{
    public List<int> ManifestIds { get; set; } = new List<int>();
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

public class CreateRouteResponse
{
    public RouteTO? Route { get; set; }
    public List<RouteDetailTO> AssignedRoutes { get; set; } = new List<RouteDetailTO>();
    public List<SOTrackingTO> CreatedTrackings { get; set; } = new List<SOTrackingTO>();
}