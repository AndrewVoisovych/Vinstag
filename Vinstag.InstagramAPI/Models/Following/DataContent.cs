using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.Following;

public record DataContent(
    [property: JsonPropertyName("user")] UserData User
);