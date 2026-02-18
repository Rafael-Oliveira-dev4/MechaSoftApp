#!/bin/bash

# MechaSoft - SQL Server Interactive Console
# Usage: ./db-console.sh

echo "🗄️  Conectando ao SQL Server..."
echo "📊 Base de Dados: DV_RO_MechaSoft"
echo "💡 Para sair: digite 'EXIT' ou pressione CTRL+C"
echo ""

docker exec -it mechasoft-sqlserver /opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "CHANGE_ME_PASSWORD" \
    -C \
    -d DV_RO_MechaSoft

