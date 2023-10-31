using Azure;
using Azure.Data.Tables;

namespace AZ.STORAGE.Entities;

public class Product : ITableEntity
{
    public string Name { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}