# MAUI Plugin System POC

This repository contains a Proof of Concept (POC) demonstrating a dynamic plugin system for .NET MAUI applications.

## Features

- **Dynamic Plugin Loading**: Load and switch plugins without restarting the API
- **JSON Configuration**: Plugin management through `plugins.json` file
- **RESTful API**: Plugin distribution and version management
- **Cross-Platform**: MAUI client with .NET API backend

## Architecture

- **MauiApp9**: Cross-platform mobile application
- **PluginAPI**: Web API for plugin management and distribution
- **Dynamic Configuration**: File-based plugin switching

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio
3. Run the PluginAPI project
4. Launch the MAUI application
5. Modify `plugins.json` to switch between plugins

## Plugin Configuration

Edit `PluginAPI/PluginAPI/plugins.json` to:
- Add new plugins
- Switch active plugins (set `isActive: true`)
- Update plugin information

## Development Notes

This is a POC with expandable features including:
- [ ] Plugin authentication
- [ ] Version management
- [ ] Plugin dependencies
- [ ] Hot-swapping capabilities
- [ ] Plugin marketplace
- [ ] Security validation
- [ ] Performance monitoring
- [ ] Plugin sandboxing

## Requirements

- .NET 8.0
- Visual Studio 2022
- MAUI workload installed