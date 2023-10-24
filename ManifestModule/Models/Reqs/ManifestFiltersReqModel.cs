namespace sectors_srv_manifest.ManifestModule.Models.Reqs;

public class ManifestFiltersReqModel
{
    public DateTime? CreatedByFrom { get; set; }
    public DateTime? CreatedByTo { get; set; }
    public string? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "CreatedAt";
    public string SortOrder { get; set; } = "desc";
}
