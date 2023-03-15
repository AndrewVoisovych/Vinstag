using Microsoft.Extensions.Options;
using Vinstag.Common.Models.Enums;
using Vinstag.Common.Models.Options;

namespace Vinstag.Common.Utils;

public class EnumHelper
{
    private readonly CsvSettingsOptions _csvSettingOptions;

    public EnumHelper(IOptions<CsvSettingsOptions> csvSettingOptions)
    {
        _csvSettingOptions = csvSettingOptions.Value;
    }

    public string GetConnectionType(ConnectionsTypes type)
    {
        return type switch
        {
            ConnectionsTypes.Followers => _csvSettingOptions.FollowersPartName,
            ConnectionsTypes.Following => _csvSettingOptions.FollowingPartName,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}