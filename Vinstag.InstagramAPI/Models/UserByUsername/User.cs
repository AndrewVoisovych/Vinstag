using System.Text.Json.Serialization;

namespace Vinstag.InstagramAPI.Models.UserByUsername;

public record User(
    [property: JsonPropertyName("pk")] string Pk,
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("full_name")] string FullName,
    [property: JsonPropertyName("is_private")] bool IsPrivate,
    [property: JsonPropertyName("profile_pic_url")] string ProfilePicUrl,
    [property: JsonPropertyName("profile_pic_id")] string ProfilePicId,
    [property: JsonPropertyName("is_verified")] bool IsVerified,
    [property: JsonPropertyName("follow_friction_type")] int FollowFrictionType,
    [property: JsonPropertyName("has_anonymous_profile_picture")] bool HasAnonymousProfilePicture,
    [property: JsonPropertyName("allowed_commenter_type")] string AllowedCommenterType,
    [property: JsonPropertyName("interop_messaging_user_fbid")] long InteropMessagingUserFbid,
    [property: JsonPropertyName("fbid_v2")] long FbidV2,
    [property: JsonPropertyName("friendship_status")] FriendshipStatus FriendshipStatus
);