#!/bin/bash

# Build the Christmas plugin DLL
echo "Building ChristmasMessagePlugin..."
dotnet build --configuration Release

# Copy the DLL to a distribution folder
mkdir -p dist
cp bin/Release/net9.0/ChristmasMessagePlugin.dll dist/

echo "Christmas Plugin built successfully!"
echo "DLL location: dist/ChristmasMessagePlugin.dll"
echo "🎄 Ready to spread Christmas joy! 🎅"