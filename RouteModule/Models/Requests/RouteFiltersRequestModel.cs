namespace sectors_srv_manifest.RouteModule.Models.Reqs;

public class RouteFiltersReq
{
    public int? CourierId { get; set; }
    public string? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Description { get; set; }
    public string SortColumn { get; set; } = "CreatedAt";
    public string SortOrder { get; set; } = "desc";
}

