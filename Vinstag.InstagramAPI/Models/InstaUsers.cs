using Vinstag.InstagramAPI.Models.Following;

namespace Vinstag.InstagramAPI.Models;

public record InstaUsers(
     IEnumerable<UserNode> Users,
     int Count
);