﻿namespace sectors_srv_manifest.RouteModule.Models.Reqs;

public class RouteFiltersReq
{
    public string? Name { get; set; }
    public string? PersonalId { get; set; }
    public string? VehiclePlate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "CreatedAt";
    public string SortOrder { get; set; } = "desc";
}

