using System.Text.Json.Serialization;
using Vinstag.InstagramAPI.Models.Following;

namespace Vinstag.InstagramAPI.Models.Followers;

public record UserData(
    [property: JsonPropertyName("edge_followed_by")] EdgeFollow EdgeFollowedBy
);