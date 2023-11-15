namespace sectors_srv_manifest.CourierModule.Models.Reqs;

public class CourierFiltersReq
{
    public string? Name { get; set; }
    public string? PersonalId { get; set; }
    public string? VehiclePlate { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "CreatedAt";
    public string SortOrder { get; set; } = "desc";
}

