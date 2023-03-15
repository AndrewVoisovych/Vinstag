using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.Following;

public record Edge(
    [property: JsonPropertyName("node")] UserNode Node
);