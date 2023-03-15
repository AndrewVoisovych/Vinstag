using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.Following;

public record EdgeFollow(
    [property: JsonPropertyName("count")] int? Count,
    [property: JsonPropertyName("page_info")] PageInfo? PageInfo,
    [property: JsonPropertyName("edges")] IReadOnlyList<Edge>? Edges
);