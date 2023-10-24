namespace sectors_srv_manifest.ManifestModule.Models;

public class PaginatedResponse<T>
{
    public int Total { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T> Items { get; set; } = new List<T>();
}
