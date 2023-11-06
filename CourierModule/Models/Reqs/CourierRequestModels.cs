namespace sectors_srv_manifest.CourierModule.Models.Reqs;

public class CreateCourierReq
{
    public string? Name { get; set; } 
    public string? PersonalId { get; set; } 
    public string? VehiclePlate { get; set; } 
}


public class UpdateCourierReq : CreateCourierReq
{
    public int Id { get; set; }
}
