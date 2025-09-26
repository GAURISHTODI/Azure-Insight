using Azure;
using Azure.Data.Tables;

namespace UrlShortener.Api;

public record UrlMapping : ITableEntity
{
    public string RowKey { get; set; } = default!; 
    public string PartitionKey { get; set; } = "links";
    public string OriginalUrl { get; set; } = default!;
    public ETag ETag { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; } = default!;
}