using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.UserByUsername;

public record RootUsers(
    [property: JsonPropertyName("users")] IReadOnlyList<Users> Users,
    [property: JsonPropertyName("status")] string Status
);