#!/bin/bash

echo "Starting Plugin API Server..."
echo "API will be available at: http://localhost:5000"
echo ""
echo "Endpoints:"
echo "  GET /plugin/version  - Get current plugin version"
echo "  GET /plugin/info     - Get plugin information"
echo "  GET /plugin/download - Download plugin DLL"
echo ""

dotnet run --urls "http://localhost:5000"