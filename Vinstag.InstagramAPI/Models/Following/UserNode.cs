using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.Following;

public record UserNode(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("full_name")] string FullName,
    [property: JsonPropertyName("profile_pic_url")] string ProfilePicUrl,
    [property: JsonPropertyName("is_private")] bool? IsPrivate,
    [property: JsonPropertyName("is_verified")] bool? IsVerified,
    [property: JsonPropertyName("followed_by_viewer")] bool? FollowedByViewer,
    [property: JsonPropertyName("requested_by_viewer")] bool? RequestedByViewer
);