using Vinstag.InstagramAPI.API;
using Vinstag.InstagramAPI.Models;

namespace Vinstag.InstagramAPI;

public interface IInstagramApi
{
    void SetSession(SessionData data);

    UsersApi Users { get; }
    StoriesApi Stories { get; }

    Task<InstaUsers> GetFollowing(EndpointsConfig config);

    Task<InstaUsers> GetFollowers(EndpointsConfig config);

    Task<InstaUsers> GetFirstFollowing(EndpointsConfig config);

    Task<InstaUsers> GetFirstFollowers(EndpointsConfig config);

}