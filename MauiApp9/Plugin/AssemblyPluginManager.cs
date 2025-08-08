using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using MauiApp9.Plugin.Contracts;

namespace MauiApp9.Plugin;

public class AssemblyPluginManager
{
    private static readonly string PluginDirectory = Path.Combine(
        FileSystem.AppDataDirectory,
        "plugins"
    );
    private static readonly string PluginInfoPath = Path.Combine(
        PluginDirectory,
        "plugin_info.json"
    );
    private static readonly HttpClient _httpClient = new();
    private static AssemblyLoadContext _pluginContext;

    public static async Task<PluginInfo> GetRemotePluginInfoAsync(string apiUrl)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MauiApp9-Plugin-Client");

            var response = await _httpClient.GetStringAsync($"{apiUrl}/plugin/info");

            // Check if response looks like JSON
            if (!response.TrimStart().StartsWith("{"))
            {
                Debug.WriteLine(
                    $"Invalid response format: {response.Substring(0, Math.Min(100, response.Length))}"
                );
                return null;
            }

            return JsonSerializer.Deserialize<PluginInfo>(response);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Plugin API Error: {ex.Message}");
            return null;
        }
    }

    public static async Task<PluginInfo> GetLocalPluginInfoAsync()
    {
        try
        {
            if (File.Exists(PluginInfoPath))
            {
                var json = await File.ReadAllTextAsync(PluginInfoPath);
                return JsonSerializer.Deserialize<PluginInfo>(json);
            }
        }
        catch { }

        return new PluginInfo { Name = "Default", Version = "0.0.0" };
    }

    public static async Task<bool> DownloadPluginAsync(PluginInfo pluginInfo)
    {
        try
        {
            Directory.CreateDirectory(PluginDirectory);

            var dllBytes = await _httpClient.GetByteArrayAsync(pluginInfo.DownloadUrl);
            var dllPath = Path.Combine(PluginDirectory, $"{pluginInfo.AssemblyName}.dll");

            await File.WriteAllBytesAsync(dllPath, dllBytes);

            var infoJson = JsonSerializer.Serialize(
                pluginInfo,
                new JsonSerializerOptions { WriteIndented = true }
            );
            await File.WriteAllTextAsync(PluginInfoPath, infoJson);

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"### ERROR: {ex}");
            return false;
        }
    }

    public static async Task<Contracts.IPluginPage> LoadPluginAsync()
    {
        try
        {
            var pluginInfo = await GetLocalPluginInfoAsync();
            var dllPath = Path.Combine(PluginDirectory, $"{pluginInfo.AssemblyName}.dll");

            System.Diagnostics.Debug.WriteLine(
                $"Loading plugin: {pluginInfo.Name} v{pluginInfo.Version}"
            );
            System.Diagnostics.Debug.WriteLine($"DLL Path: {dllPath}");
            System.Diagnostics.Debug.WriteLine($"DLL Exists: {File.Exists(dllPath)}");

            if (File.Exists(dllPath))
            {
                UnloadPreviousPlugin();

                _pluginContext = new AssemblyLoadContext("PluginContext", true);
                var assembly = _pluginContext.LoadFromAssemblyPath(dllPath);

                System.Diagnostics.Debug.WriteLine($"Assembly loaded: {assembly.FullName}");
                System.Diagnostics.Debug.WriteLine($"Looking for type: {pluginInfo.TypeName}");

                var pluginType = assembly.GetType(pluginInfo.TypeName);
                if (
                    pluginType != null
                    && typeof(Contracts.IPluginPage).IsAssignableFrom(pluginType)
                )
                {
                    var instance = (Contracts.IPluginPage)Activator.CreateInstance(pluginType);
                    System.Diagnostics.Debug.WriteLine(
                        $"Plugin instance created: {instance.PluginName} v{instance.Version}"
                    );
                    return instance;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Plugin type not found or invalid: {pluginType?.Name}"
                    );
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Plugin loading error: {ex.Message}");
        }

        System.Diagnostics.Debug.WriteLine("Falling back to default plugin");
        return new DefaultPluginPage();
    }

    public static bool HasUpdate(PluginInfo local, PluginInfo remote)
    {
        if (remote == null)
            return false;

        var localVersion = Version.Parse(local.Version);
        var remoteVersion = Version.Parse(remote.Version);

        return remoteVersion > localVersion;
    }

    private static void UnloadPreviousPlugin()
    {
        if (_pluginContext != null)
        {
            _pluginContext.Unload();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
