namespace sectors_srv_manifest.TrackingModule.Models.Reqs;

public class TrackingFiltersReq
{
    public int? ServiceOrderId { get; set; }
    public string? SOExternalId { get; set; }
    public string? EvExCode { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "Date";
    public string SortOrder { get; set; } = "desc";
}

