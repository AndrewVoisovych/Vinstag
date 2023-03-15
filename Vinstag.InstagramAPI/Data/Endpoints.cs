namespace Vinstag.InstagramAPI.Data;

public static class Endpoints
{
    public const string BaseAdress = "https://www.instagram.com/";
    public const string ApiAddress = "https://i.instagram.com/";

    public const string Following = "graphql/query/";
    public const string FollowingQueryHash = "d04b0a864b4b54837c0d870b0e77e076";
    public const string FollowersQueryHash = "c76146de99bb02f6415203be841dd25a";

    public const string SearchFromUsername = "web/search/topsearch/";
    public const string SearchFromId = ApiAddress + "api/v1/users/{0}/info/";

    public const string StoryViewers = ApiAddress + "api/v1/media/{0}/list_reel_media_viewer/";



    public const string UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_2_6 like Mac OS X) " +
                                    "AppleWebKit/604.5.6 (KHTML, like Gecko) Mobile/15D100 " +
                                    "Instagram 37.0.0.9.96 " +
                                    "(iPhone7,2; iOS 11_2_6; pt_PT; pt-PT; scale=2.34; gamut=normal; 750x1331)";
}