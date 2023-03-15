namespace Vinstag.Common.Models.Storage;

public record StorageLog(
    string DateTime,
    string Action,
    string Name,
    string Id,
    string Type,
    int NumberOf
);