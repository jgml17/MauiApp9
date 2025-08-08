#!/bin/bash

# Build the plugin DLL
echo "Building CustomMessagePlugin..."
dotnet build --configuration Release

# Copy the DLL to a distribution folder
mkdir -p dist
cp bin/Release/net9.0/CustomMessagePlugin.dll dist/

echo "Plugin built successfully!"
echo "DLL location: dist/CustomMessagePlugin.dll"
echo "Upload this DLL to your server and update the API endpoint."