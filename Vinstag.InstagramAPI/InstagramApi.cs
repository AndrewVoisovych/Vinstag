using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;
using Vinstag.Common.DependencyInjection;
using Vinstag.InstagramAPI.API;
using Vinstag.InstagramAPI.Exception;
using Vinstag.InstagramAPI.HttpTransaction;
using Vinstag.InstagramAPI.Models;
using Vinstag.InstagramAPI.Models.Abstractions;
using Vinstag.InstagramAPI.Models.Followers;
using Vinstag.InstagramAPI.Models.Following;

namespace Vinstag.InstagramAPI;

// TODO: FIX session data
public class InstagramApi : IInstagramApi
{
    public UsersApi Users { get; private set; }
    public StoriesApi Stories { get; private set; }

    private readonly IHttpClientFactory _httpClientFactory;
    private SessionData _sessionData;


    public InstagramApi(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        InitializeBranches();
    }

    private HttpClient HttpClient => _httpClientFactory.CreateClient("instagram");

    public void SetSession(SessionData data)
    {
        // TODO: Check all its fine? 
        // TODO: BUILDER maybe?
        _sessionData = data;
    }

    private void InitializeBranches()
    {
        Users = new UsersApi(HttpClient);
        Stories = new StoriesApi(HttpClient);
    }

    public async Task<InstaUsers> GetFollowing(EndpointsConfig config)
    {
        var receivedData = await GetFirstUsers<FollowingRoot>(config);
        var endCursor = receivedData?.Data?.User.EdgeFollow.PageInfo?.EndCursor;

        List<UserNode>? users = receivedData?.Data?.User.EdgeFollow.Edges?
            .Select(x => x.Node).ToList();

        var usersCount = receivedData?.Data?.User?.EdgeFollow?.Count;

        while (!string.IsNullOrEmpty(endCursor))
        {
            config.NextQueryHash = endCursor;
            var nextPage = await GetFirstUsers<FollowingRoot>(config);
            endCursor = nextPage?.Data?.User.EdgeFollow.PageInfo?.EndCursor;
            var newUsersList = nextPage?.Data?.User.EdgeFollow.Edges?
                .Select(x => x.Node).ToList();

            if (newUsersList != null)
            {
                users?.AddRange(newUsersList);
            }
        }

        if (users is null || usersCount == null)
        {
            throw new NoUsersException("Failed to get users");
        }

        return new InstaUsers(users, (int)usersCount);
    }

    public async Task<InstaUsers> GetFollowers(EndpointsConfig config)
    {
        var receivedData = await GetFirstUsers<FollowersRoot>(config);
        var endCursor = receivedData?.Data?.User.EdgeFollowedBy.PageInfo?.EndCursor;

        List<UserNode>? users = receivedData?.Data?.User.EdgeFollowedBy.Edges?
            .Select(x => x.Node).ToList();

        var usersCount = receivedData?.Data?.User?.EdgeFollowedBy?.Count;

        while (!string.IsNullOrEmpty(endCursor))
        {
            config.NextQueryHash = endCursor;
            var nextPage = await GetFirstUsers<FollowersRoot>(config);
            endCursor = nextPage?.Data?.User.EdgeFollowedBy.PageInfo?.EndCursor;
            var newUsersList = nextPage?.Data?.User.EdgeFollowedBy?.Edges?
                .Select(x => x.Node).ToList();

            if (newUsersList != null)
            {
                users?.AddRange(newUsersList);
            }
        }
        
        if (users is null || usersCount == null)
        {
            throw new NoUsersException("Failed to get users");
        }

        return new InstaUsers(users, (int)usersCount);
    }

    public async Task<InstaUsers> GetFirstFollowing(EndpointsConfig config)
    {
        //TODO:  func delegate and merge couple of similar functions
        var receivedData = await GetFirstUsers<FollowingRoot>(config);
        
        var users = receivedData?.Data?.User.EdgeFollow.Edges?
            .Select(x => x.Node).ToList();

        var usersCount = receivedData?.Data?.User.EdgeFollow.Count;
        if (users is null || usersCount == null)
        {
            throw new NoUsersException("Failed to get users");
        }

        return new InstaUsers(users, (int)usersCount);
    }

    public async Task<InstaUsers> GetFirstFollowers(EndpointsConfig config)
    {
        var receivedData = await GetFirstUsers<FollowersRoot>(config);

        var users = receivedData?.Data?.User.EdgeFollowedBy.Edges?
            .Select(x => x.Node).ToList();

        var usersCount = receivedData?.Data?.User.EdgeFollowedBy.Count;
        if (users is null || usersCount == null)
        {
            throw new NoUsersException("Failed to get users");
        }

        return new InstaUsers(users, (int) usersCount);

    }

    private async Task<T?> GetFirstUsers<T>(EndpointsConfig config) where T : IUsers
    {
        string queryParams = SetQuery(config.EndpointHash!, config.NextQueryHash);

        HttpRequestMessage requestMessage = new(HttpMethod.Get, config.Endpoint + queryParams)
        {
            Headers =
            {
                {HeaderNames.Cookie, _sessionData.Cookies}
            }
        };

        var response = await Request.ExecuteRequest(HttpClient, requestMessage);
        var receivedData = await response.Content.ReadFromJsonAsync<T>();

        if (receivedData is not null &&
            receivedData?.Status != null &&
            receivedData.Status.ContainsInvariant("ok")
            // TODO: No correct COOKIE
           )
        {
            return receivedData;
        }

        throw new ArgumentException("Failed to read data");
    }


    private string SetQuery(string endpointHash, string nextQueryHash = "")
    {
        Dictionary<string, string> queryParamsDictionary = new()
        {
            {"query_hash", endpointHash},
            {
                "variables", "{" +
                          "\"id\":" + _sessionData.UserId + "," +
                          "\"first\":50" +
                          "}"
            }
        };

        if (!string.IsNullOrEmpty(nextQueryHash))
        {
            queryParamsDictionary["variables"] = "{" +
                                                 "\"id\":" + _sessionData.UserId + "," +
                                                 "\"first\":50, " +
                                                 "\"after\":\"" + nextQueryHash + "\"" +
                                                 "}";
        }

        return new QueryBuilder(queryParamsDictionary).ToString();
    }
    
}