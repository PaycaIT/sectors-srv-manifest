﻿namespace sectors_srv_manifest.RouteModule.Models.Reqs;

public class RouteFiltersReq
{
    public string StartingManifestId { get; set; } = string.Empty;
    public int CourierId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "CreatedAt";
    public string SortOrder { get; set; } = "desc";
}
