using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.Following;

public record PageInfo(
    [property: JsonPropertyName("has_next_page")] bool? HasNextPage,
    [property: JsonPropertyName("end_cursor")] string EndCursor
);