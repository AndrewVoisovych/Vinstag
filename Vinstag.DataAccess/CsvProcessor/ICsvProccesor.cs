using Vinstag.Common.Models.Storage;

namespace Vinstag.DataAccess.CsvProcessor;

// TODO: Global name or part of BIG data storage (SQL LITE, GOOGLE CLOUD, CSV FILE PROCCESOR)
public interface IDataProccesor
{
    Task<bool> CreateData<T>(StorageConfig<T> config, string filePath = "");
    Task<IEnumerable<T>> ReadData<T>(string id, string type);
    Task<List<StorageLog>> LogDataDelegation(StorageConfig<StorageUser> config,
        IEnumerable<StorageUser> addedUsers, IEnumerable<StorageUser> removedUsers);
}