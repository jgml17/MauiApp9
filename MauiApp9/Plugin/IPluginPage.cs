using System.Text.Json.Serialization;

namespace MauiApp9.Plugin;

public interface IPluginPage
{
    string PluginName { get; }
    string Version { get; }
    ContentPage CreatePage();
}

public class PluginInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    
    [JsonPropertyName("version")]
    public string Version { get; set; } = "1.0.0";
    
    [JsonPropertyName("downloadUrl")]
    public string DownloadUrl { get; set; } = "";
    
    [JsonPropertyName("assemblyName")]
    public string AssemblyName { get; set; } = "";
    
    [JsonPropertyName("typeName")]
    public string TypeName { get; set; } = "";
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}