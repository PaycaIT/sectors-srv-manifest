﻿using System.Text.Json.Serialization;
namespace sectors_srv_manifest.RouteModule.Models;

public class RouteTO
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public int CourierId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public string? Description { get; set; } = string.Empty;

    [JsonIgnore]
    public int ClientId { get; set; }

}

public class RouteDetailTO
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int ServiceOrderId { get; set; }
    [JsonIgnore]
    public int ClientId { get; set; }
    public DateTime CreatedAt { get; set; }
}


