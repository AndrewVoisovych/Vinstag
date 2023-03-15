using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.UserByUsername;

public record FriendshipStatus(
    [property: JsonPropertyName("following")] bool Following,
    [property: JsonPropertyName("is_private")] bool IsPrivate,
    [property: JsonPropertyName("incoming_request")] bool IncomingRequest,
    [property: JsonPropertyName("outgoing_request")] bool OutgoingRequest,
    [property: JsonPropertyName("is_bestie")] bool IsBestie,
    [property: JsonPropertyName("is_restricted")] bool IsRestricted,
    [property: JsonPropertyName("is_feed_favorite")] bool IsFeedFavorite
);