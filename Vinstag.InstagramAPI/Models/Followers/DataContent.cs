using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.Followers;

public record DataContent(
    [property: JsonPropertyName("user")] UserData User
);