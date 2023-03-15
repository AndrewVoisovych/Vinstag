using Microsoft.Net.Http.Headers;
using Vinstag.InstagramAPI.Data;
using static System.String;

namespace Vinstag.InstagramAPI.API;

public class StoriesApi
{
    private readonly HttpClient httpClient;

    public StoriesApi(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public void GetStoryViewer(string mediaId)
    {
        string endpoint = Format(Endpoints.SearchFromId, mediaId);

        HttpRequestMessage requestMessage = new(HttpMethod.Get, endpoint)
        {
            Headers =
            {
                
                {HeaderNames.UserAgent, Endpoints.UserAgent},
            }
        };

        //var response = await Request.ExecuteRequest(httpClient, requestMessage);
        //var receivedData = await response.Content.ReadFromJsonAsync<Models.UserById.RootUsers>();

        //if (receivedData?.Status == null ||
        //    !receivedData.Status.ContainsInvariant("ok") ||
        //    receivedData.User?.Pk is null
        //   )
        //{
        //    throw new NoUsersException($"User with id '{id}' not found");
        //}

        
    }
}