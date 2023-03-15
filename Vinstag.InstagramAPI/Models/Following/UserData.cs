using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.Following;

public record UserData(
    [property: JsonPropertyName("edge_follow")] EdgeFollow EdgeFollow
);