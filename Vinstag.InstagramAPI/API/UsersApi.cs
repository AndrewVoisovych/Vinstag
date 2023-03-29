using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;
using Vinstag.Common.DependencyInjection;
using Vinstag.InstagramAPI.Data;
using Vinstag.InstagramAPI.Exception;
using Vinstag.InstagramAPI.HttpTransaction;
using Vinstag.InstagramAPI.Models;
using Vinstag.InstagramAPI.Models.UserByUsername;
using static System.String;

namespace Vinstag.InstagramAPI.API;

public class UsersApi
{
    private readonly HttpClient httpClient;
    private readonly SessionData session;


    public UsersApi(HttpClient httpClient, SessionData session)
    {
        this.httpClient = httpClient;
        this.session = session;
    }

    public async Task<User> GetUserByUserName(string username, string endpoint)
    {
        string queryParams = new QueryBuilder(
            new Dictionary<string, string>()
            {
                { "query", username }
            }
            ).ToString();

        HttpRequestMessage requestMessage = new(HttpMethod.Get, endpoint + queryParams)
        {
            Headers =
            {

                { HeaderNames.Cookie,  session.Cookies },
            }
        };

        
        var response = await Request.ExecuteRequest(httpClient, requestMessage);
        var receivedData = await response.Content.ReadFromJsonAsync<RootUsers>();


        if (receivedData?.Status == null ||
            !receivedData.Status.ContainsInvariant("ok") ||
            receivedData.Users.Count < 1
           )
        {
            throw new NoUsersException($"User with username '{username}' not found");
        }

        var result = receivedData.Users.First(x => x.Position == 0).User;

        return result;
    }

    public async Task<string> GetUserNameById(string id)
    {
        var user = await GetUserById(id);

        return user.Username;
    }

    public async Task<Models.UserById.User> GetUserById(string id)
    {
        string endpoint = Format(Endpoints.SearchFromId, id);

        HttpRequestMessage requestMessage = new(HttpMethod.Get, endpoint)
        {
            Headers =
            {
                {HeaderNames.UserAgent, Endpoints.UserAgent}
            },
            RequestUri = new Uri(endpoint)
        };

        var response = await Request.ExecuteRequest(httpClient, requestMessage);
        var receivedData = await response.Content.ReadFromJsonAsync<Models.UserById.RootUsers>();

        if (receivedData?.Status == null ||
            !receivedData.Status.ContainsInvariant("ok") ||
            receivedData.User?.Pk is null
           )
        {
            throw new NoUsersException($"User with id '{id}' not found");
        }

        return receivedData.User;
    }
}

