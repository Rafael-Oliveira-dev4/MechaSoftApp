#!/bin/bash
echo "🛑 Parando MechaSoft App..."

# Parar processos .NET
pkill -f "dotnet run"

# Parar processos npm
pkill -f "npm start"

# Parar SQL Server
docker stop mechasoft-sqlserver

echo "✅ MechaSoft App parado!"
