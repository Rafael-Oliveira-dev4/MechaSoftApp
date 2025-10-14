#!/bin/bash

# MechaSoft - Query SQL Server Database
# Usage: ./db-query.sh "SELECT * FROM [MechaSoftCS].[Users]"

QUERY="$1"

if [ -z "$QUERY" ]; then
    echo "❌ Forneça uma query SQL"
    echo "Uso: ./db-query.sh \"SELECT * FROM [MechaSoftCS].[Users]\""
    exit 1
fi

docker exec mechasoft-sqlserver /opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "CHANGE_ME_PASSWORD" \
    -C \
    -d DV_RO_MechaSoft \
    -Q "$QUERY"

