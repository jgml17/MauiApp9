using System.Text.Json;
using PluginAPI.Models;

namespace PluginAPI.Services;

public class PluginConfigService
{
    private readonly string _configPath;
    private PluginConfig? _config;
    private DateTime _lastModified;

    public PluginConfigService(IWebHostEnvironment environment)
    {
        _configPath = Path.Combine(environment.ContentRootPath, "plugins.json");
    }

    public PluginConfig GetConfig()
    {
        var fileInfo = new FileInfo(_configPath);
        
        if (_config == null || fileInfo.LastWriteTime > _lastModified)
        {
            var json = File.ReadAllText(_configPath);
            _config = JsonSerializer.Deserialize<PluginConfig>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            _lastModified = fileInfo.LastWriteTime;
        }

        return _config ?? new PluginConfig { Plugins = new List<PluginInfo>() };
    }

    public PluginInfo? GetActivePlugin()
    {
        return GetConfig().Plugins.FirstOrDefault(p => p.IsActive);
    }
}