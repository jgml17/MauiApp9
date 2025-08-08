using Microsoft.AspNetCore.Mvc;
using PluginAPI.Models;
using PluginAPI.Services;

namespace PluginAPI.Controllers;

[ApiController]
[Route("plugin")]
public class PluginController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly PluginConfigService _configService;

    public PluginController(IWebHostEnvironment environment, PluginConfigService configService)
    {
        _environment = environment;
        _configService = configService;
    }

    [HttpGet("version")]
    public ActionResult<VersionInfo> GetVersion()
    {
        var activePlugin = _configService.GetActivePlugin();
        return Ok(new VersionInfo { Version = activePlugin?.Version ?? "1.0.0" });
    }

    [HttpGet("info")]
    public ActionResult<PluginInfo> GetPluginInfo()
    {
        var activePlugin = _configService.GetActivePlugin();
        if (activePlugin == null)
            return NotFound("No active plugin found");

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        activePlugin.DownloadUrl = $"{baseUrl}/plugin/download";
        
        return Ok(activePlugin);
    }

    [HttpGet("download")]
    public IActionResult DownloadPlugin()
    {
        var activePlugin = _configService.GetActivePlugin();
        if (activePlugin == null)
            return NotFound("No active plugin found");

        var pluginPath = Path.Combine(_environment.ContentRootPath, "plugins", activePlugin.FileName);
        
        if (!System.IO.File.Exists(pluginPath))
            return NotFound("Plugin DLL not found");

        var fileBytes = System.IO.File.ReadAllBytes(pluginPath);
        return File(fileBytes, "application/octet-stream", activePlugin.FileName);
    }
}
