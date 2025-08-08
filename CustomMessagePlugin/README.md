# Custom Message Plugin

This is a sample plugin for the MauiApp9 plugin system.

## Building the Plugin

1. Run the build script:
   ```bash
   ./build.sh
   ```

2. The compiled DLL will be in `dist/CustomMessagePlugin.dll`

## Deployment

1. Upload `CustomMessagePlugin.dll` to your server
2. Update your API endpoint to return:
   ```json
   {
     "Name": "Custom Message Plugin",
     "Version": "2.1.0",
     "DownloadUrl": "https://your-server.com/plugins/CustomMessagePlugin.dll",
     "AssemblyName": "CustomMessagePlugin",
     "TypeName": "MauiApp9.Plugins.CustomMessagePlugin",
     "Description": "Enhanced message plugin with custom UI"
   }
   ```

## Plugin Features

- Advanced dashboard UI with gradient background
- Statistics cards with real-time updates
- Interactive message list with data binding
- CollectionView with custom item templates
- Add/Clear functionality with confirmations
- Modern card-based design with shadows
- Emoji icons and color-coded elements

## Creating New Plugins

1. Copy this project structure
2. Implement `IPluginPage` interface
3. Update plugin name and version
4. Build and deploy DLL