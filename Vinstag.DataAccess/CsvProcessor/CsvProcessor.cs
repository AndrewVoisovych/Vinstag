using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using System.Globalization;
using Vinstag.Common.DependencyInjection;
using Vinstag.Common.Models.Enums;
using Vinstag.Common.Models.Options;
using Vinstag.Common.Models.Storage;
using Vinstag.Common.Utils;
using static System.String;

namespace Vinstag.DataAccess.CsvProcessor;

public class CsvProcessor : IDataProccesor
{
    private readonly CsvConfiguration _config;
    private readonly CsvSettingsOptions _csvSettingsOptions;
    private readonly EnumHelper _enumHelper;

    public CsvProcessor(
        IOptions<CsvSettingsOptions> csvSettingsOptions,
        EnumHelper enumHelper
    )
    {
        _csvSettingsOptions = csvSettingsOptions.Value;
        _enumHelper = enumHelper;

        _config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            NewLine = Environment.NewLine,
        };
    }

    public async Task<List<StorageLog>> LogDataDelegation(
        StorageConfig<StorageUser> config,
        IEnumerable<StorageUser> addedUsers, 
        IEnumerable<StorageUser> removedUsers)
    {
        string typeString = _enumHelper.GetConnectionType(config.ConnectionsType);

        List<StorageLog> followStorageLogs = ConvertToStorageLogs(addedUsers,
            UserActions.Follow.ToString(), typeString, config.RecordsCount).ToList();

        List<StorageLog> unfollowStorageLogs = ConvertToStorageLogs(removedUsers,
            UserActions.UnFollow.ToString(), typeString, config.RecordsCount).ToList();

        List<StorageLog> storageLogs = followStorageLogs.Concat(unfollowStorageLogs).ToList();

        var filePath = await GetFilePath(config.Id, _csvSettingsOptions.LogFilename, 0);

        var logStorageConfig = new StorageConfig<StorageLog>
        {
            Id = config.Id,
            ConnectionsType = config.ConnectionsType,
            Records = storageLogs,
            RecordsCount = config.RecordsCount
        };

        bool isSavedLogs = File.Exists(filePath)
            ? await UpdateData(logStorageConfig)
            : await CreateData(logStorageConfig, filePath);

        if (isSavedLogs)
        {
            await CreateData(config);
        }

        return storageLogs;
    }


    public async Task<bool> UpdateData<T>(StorageConfig<T> config)
    {
        var savedUsers = (await ReadData<T>(config.Id, _csvSettingsOptions.LogFilename)).ToList();
        var records = config.Records.Concat(savedUsers).ToList();

        var filePath = await GetFilePath(config.Id, _csvSettingsOptions.LogFilename, 0);
        await using var writer = new StreamWriter(filePath);

        await using var csv = new CsvWriter(writer, _config);
        await csv.WriteRecordsAsync(records);

        return true;
    }


    public async Task<bool> CreateData<T>(StorageConfig<T> config, string filePath = "")
    {
        var inputData = config.Records.ToList();

        if (IsNullOrEmpty(filePath))
        {
            string action = _enumHelper.GetConnectionType(config.ConnectionsType);
            filePath = await GetFilePath(config.Id, action, config.RecordsCount);
        }

        await using var writer = new StreamWriter(filePath);

        await using var csv = new CsvWriter(writer, _config);
        await csv.WriteRecordsAsync(inputData);

        return true;
    }

    public async Task<IEnumerable<T>> ReadData<T>(string id, string action)
    {
        string filePath = await Task.Run(() => SearchFilePath(id, action));

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, _config);
        var records = await csv.GetRecordsAsync<T>().ToListAsync();

        return records;
    }



    #region Path / Filename

    private string GetDirectoryPath(string id) => $"{_csvSettingsOptions.DirectoryPath}{id}/";

    private string GetFileName(string id, string type, int recordCount)
        => $"{type}" +
          $"{(recordCount != 0 ? _csvSettingsOptions.Delimiter + recordCount : "")}" +
          $"{_csvSettingsOptions.Delimiter}{id}{_csvSettingsOptions.FilenameExtension}";


    private async Task<string> GetFilePath(string id, string action, int recordCount)
    {
        string folderPath = GetDirectoryPath(id);

        await Task.Run(() =>
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        });

        return folderPath + GetFileName(id, action, recordCount);
    }

    private string SearchFilePath(string id, string action)
    {
        string folderPath = GetDirectoryPath(id);
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"Can't find data about '{id}' on this path: {folderPath}");
        }

        var filePath = Directory
            .GetFiles(folderPath)
            .FirstOrDefault(p =>
                p.ContainsInvariant(action) && p.ContainsInvariant(id));

        if (IsNullOrEmpty(filePath))
        {
            throw new FileNotFoundException($"Can't find data about this action: {action}", filePath);
        }

        return filePath;
    }



    #endregion

    private IEnumerable<StorageLog> ConvertToStorageLogs(IEnumerable<StorageUser> storageUsers,
        string action, string type, int storageUsersCount)
    {
        var storageUsersList = storageUsers.ToList();
        var currentDateTime = DateTime.Now.ToString(_csvSettingsOptions.DateTimeMask);

        var storageLogs = storageUsersList
            .Select(r =>
                new StorageLog(
                    DateTime: currentDateTime,
                    Action: action,
                    Name: r.Username,
                    Id: r.Id,
                    Type: type,
                    NumberOf: storageUsersCount
                )
            );

        return storageLogs;
    }
}