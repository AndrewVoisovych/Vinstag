namespace Vinstag.InstagramAPI.HttpTransaction;

public static class Request
{
    public static async Task<HttpResponseMessage> ExecuteRequest(HttpClient httpClient, HttpRequestMessage requestMessage)
    {
        var response = await httpClient.SendAsync(requestMessage);

        return response.IsSuccessStatusCode
            ? response
            : throw new HttpRequestException("The request was not completed");
    }
}