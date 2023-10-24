namespace sectors_srv_manifest.ManifestModule.Models;

public class CourierModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string PersonalId { get; set; } = "";
    public string VehiclePlate { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
