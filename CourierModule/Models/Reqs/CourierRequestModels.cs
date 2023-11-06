namespace sectors_srv_manifest.CourierModule.Models.Reqs;

public class CreateCourierReq
{
    public string Name { get; set; } = string.Empty;
    public string PersonalId { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty; 
}


public class UpdateCourierReq : CreateCourierReq
{
    public int Id { get; set; }
}
