using Vinstag.InstagramAPI;
using Vinstag.InstagramAPI.Models;

namespace Vinstag.API.Services;

public class AuthenticationService
{
    private readonly IInstagramApi _instaApi;

    public AuthenticationService(IInstagramApi instaApi)
    {
        _instaApi = instaApi;
    }

    public string SignIn(string authKey)
    {
        if (IsDecoded(authKey))
        {
            authKey = Uri.UnescapeDataString(authKey);
        }
       
        var id = !String.IsNullOrWhiteSpace(authKey.Split(':')[0])
            ? authKey.Split(':')[0]
            : "0";

        _instaApi.SetSession(new SessionData
        {
            UserId = id,
            Cookies = $"sessionid={authKey}"
        });

        return id;
    }



    private static bool IsDecoded(string input)
    {
        return input != Uri.UnescapeDataString(input);
    }
}