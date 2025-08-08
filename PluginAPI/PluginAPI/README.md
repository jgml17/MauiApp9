# Plugin API Server

ASP.NET Core Web API for serving MAUI plugins.

## Running the Server

```bash
cd PluginAPI
bash run.sh
```

Server will start at: `http://localhost:5000`

## API Endpoints

### GET /plugin/version
Returns current plugin version
```json
{
  "Version": "2.1.0"
}
```

### GET /plugin/info
Returns plugin information
```json
{
  "Name": "Custom Message Plugin",
  "Version": "2.1.0",
  "DownloadUrl": "http://localhost:5000/plugin/download",
  "AssemblyName": "CustomMessagePlugin",
  "TypeName": "MauiApp9.Plugins.CustomMessagePlugin",
  "Description": "Enhanced message plugin with custom UI and features"
}
```

### GET /plugin/download
Downloads the plugin DLL file

## Updating Plugins

1. Build new plugin DLL
2. Copy to `PluginAPI/plugins/` directory
3. Update version in `PluginController.cs`
4. Restart server

## Testing

Use Swagger UI at: `http://localhost:5000/swagger`

## Mobile App Configuration

Update your mobile app to use: `http://localhost:5000` as the API URL