using Vinstag.InstagramAPI;
using Vinstag.InstagramAPI.Data;

namespace Vinstag.API.Services;

public class UserService
{
    private readonly IInstagramApi _instaApi;

    public UserService(IInstagramApi instaApi)
    {
        _instaApi = instaApi;
    }

    public async Task<string> GetIdByUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            username = "andr_vois";
        }

        var user = await _instaApi.Users.GetUserByUserName(username, Endpoints.SearchFromUsername);

        return user.Pk;
    }
}
