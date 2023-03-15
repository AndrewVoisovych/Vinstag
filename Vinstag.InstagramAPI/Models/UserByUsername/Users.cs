using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.UserByUsername;

public record Users(
    [property: JsonPropertyName("position")] int Position,
    [property: JsonPropertyName("user")] User User
);