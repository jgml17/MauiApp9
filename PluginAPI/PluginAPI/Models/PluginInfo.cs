namespace PluginAPI.Models;

public class PluginInfo
{
    public string Name { get; set; } = "";
    public string Version { get; set; } = "1.0.0";
    public string DownloadUrl { get; set; } = "";
    public string AssemblyName { get; set; } = "";
    public string TypeName { get; set; } = "";
    public string Description { get; set; } = "";
    public string FileName { get; set; } = "";
    public bool IsActive { get; set; }
}

public class PluginConfig
{
    public List<PluginInfo> Plugins { get; set; } = new();
}

public class VersionInfo
{
    public string Version { get; set; } = "1.0.0";
}