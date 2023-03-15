using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.UserById;

public record RootUsers(
    [property: JsonPropertyName("user")] User User,
    [property: JsonPropertyName("status")] string Status
);