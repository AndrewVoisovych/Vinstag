namespace Vinstag.Common.Models.Options;

public class CsvSettingsOptions
{
    public string DirectoryPath { get; set; }
    public string FollowingPartName { get; set; }
    public string FollowersPartName { get; set; }
    public string Delimiter { get; set; }
    public string FilenameExtension { get; set; }

    public string LogFilename { get; set; }
    
    public string DateTimeMask { get; set; }
}