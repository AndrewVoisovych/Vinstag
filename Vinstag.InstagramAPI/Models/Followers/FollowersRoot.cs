using System.Text.Json.Serialization;
using Vinstag.InstagramAPI.Models.Abstractions;

namespace Vinstag.InstagramAPI.Models.Followers;

public record FollowersRoot(
    [property: JsonPropertyName("data")] DataContent? Data,
    [property: JsonPropertyName("status")] string? Status
): IUsers;