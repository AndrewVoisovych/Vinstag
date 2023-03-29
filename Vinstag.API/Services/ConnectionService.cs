using Vinstag.Common.Models.Enums;
using Vinstag.Common.Models.Storage;
using Vinstag.DataAccess.CsvProcessor;
using Vinstag.InstagramAPI;
using Vinstag.InstagramAPI.Data;
using Vinstag.InstagramAPI.Models;
using Vinstag.InstagramAPI.Models.Following;

namespace Vinstag.API.Services;

public class ConnectionService
{
    private readonly IInstagramApi _instaApi;
    private readonly IDataProccesor _dataProccesor;

    public ConnectionService(
        IInstagramApi instaApi,
        IDataProccesor dataProccesor
    )
    {
        _instaApi = instaApi;
        _dataProccesor = dataProccesor;
    }

    public async Task<InstaUsers?> GetFollowing(string id, bool onlyFirst = true, bool saveData = false)
    {
        var endpointConfig = new EndpointsConfig
        {
            Id = id,
            Endpoint = Endpoints.Following,
            EndpointHash = Endpoints.FollowingQueryHash
        };

        InstaUsers usersData = onlyFirst
            ? await _instaApi.GetFirstFollowing(endpointConfig)
            : await _instaApi.GetFollowing(endpointConfig);

        var users = usersData?.Users?.ToList();

        if (users != null && saveData && users.Any())
        {
            await SaveData(users, id, ConnectionsTypes.Following, usersData!.Count);
        }

        return usersData;
    }

    public async Task<InstaUsers?> GetFollowers(string id, bool onlyFirst = true, bool saveData = false)
    {
        var endpointConfig = new EndpointsConfig
        {
            Id = id,
            Endpoint = Endpoints.Following,
            EndpointHash = Endpoints.FollowersQueryHash
        };

        InstaUsers usersData = onlyFirst
            ? await _instaApi.GetFirstFollowers(endpointConfig)
            : await _instaApi.GetFollowers(endpointConfig);

        var users = usersData?.Users?.ToList();

        if (users != null && saveData && users.Any())
        {
            await SaveData(users, id, ConnectionsTypes.Followers, usersData!.Count);
        }

        return usersData;
    }

    public async Task<List<StorageLog>?> GetDifferents(string id, ConnectionsTypes type)
    {
        List<StorageUser> savedUsers =
            (await _dataProccesor.ReadData<StorageUser>(id, type.ToString())).ToList();


        InstaUsers? receivedUsers = type == ConnectionsTypes.Following
            ? await GetFollowing(id, false)
            : await GetFollowers(id, false);

        if (receivedUsers?.Users is null)
        {
            return null;
        }

        var userData = receivedUsers.Users!.ToList();

        if (!savedUsers.Any())
        {
            await SaveData(userData, id, type, receivedUsers!.Count);

            return null;
        }


        HashSet<string> savedUsersSet = new(savedUsers.Select(x => x.Id));
        HashSet<string> receivedUsersSet = new(userData.Select(x => x.Id));

        var beganFollow = receivedUsersSet.Except(savedUsersSet).ToList();
        var unFollow = savedUsersSet.Except(receivedUsersSet).ToList();

        if (!beganFollow.Any() && !unFollow.Any())
        {
            return new List<StorageLog>();
        }

        var beganFollowUserToStorage = await ConvertToStorageUser(beganFollow, userData);
        var unFollowFollowUserToStorage = await ConvertToStorageUser(unFollow, userData);

        var storageConfig = new StorageConfig<StorageUser>
        {
            Id = id,
            ConnectionsType = type,
            Records = new List<StorageUser>(),
            RecordsCount = receivedUsers?.Count ?? 0
        };

        var storageLogs = await _dataProccesor.LogDataDelegation
            (storageConfig, beganFollowUserToStorage, unFollowFollowUserToStorage);


        return storageLogs;
    }

    private async Task<List<StorageUser>> ConvertToStorageUser(List<string> ids, List<UserNode> receivedUsers)
    {
        var storageUsers = await Task.WhenAll(
            ids.Select(async x =>
                new StorageUser(
                    Id: x,
                    Username:
                    receivedUsers.FirstOrDefault(r => r.Id == x)?.Username ??
                    await _instaApi.Users.GetUserNameById(x)
                )
            )
        );

        return storageUsers.ToList();

    }

    private async Task<bool> SaveData(List<UserNode> userData, string id, ConnectionsTypes type, int userCount)
    {
        IEnumerable<StorageUser> mappedUsers = userData
            .Select(u => new StorageUser(u.Username, u.Id));

        var storageUserConfig = new StorageConfig<StorageUser>
        {
            Id = id,
            ConnectionsType = type,
            Records = mappedUsers,
            RecordsCount = userCount
        };

        bool isSaved = await _dataProccesor.CreateData(storageUserConfig);

        return isSaved;
    }
}

