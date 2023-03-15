using Vinstag.Common.Models.Enums;

namespace Vinstag.Common.Models.Storage;

public record StorageConfig<T>
{
    public string Id { get; init; }
    public ConnectionsTypes ConnectionsType { get; set; }
    public IEnumerable<T> Records { get; set; }
    public int RecordsCount { get; set; }
};
