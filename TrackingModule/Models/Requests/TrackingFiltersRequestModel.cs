namespace sectors_srv_manifest.TrackingModule.Models.Reqs;

public class TrackingFiltersReq
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ServiceOrderId { get; set; }
    public int SOExternalId { get; set; }
    public string EvExCode { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "CreatedAt";
    public string SortOrder { get; set; } = "desc";
}

