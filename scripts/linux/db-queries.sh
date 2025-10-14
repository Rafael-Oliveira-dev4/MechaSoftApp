#!/bin/bash

# MechaSoft - Queries Úteis para SQL Server
# Usage: ./db-queries.sh [comando]

DB_QUERY="/home/rick/Documents/GitHub/MechaSoftApp/scripts/linux/db-query.sh"

case "$1" in
    users|utilizadores)
        echo "📋 Listando utilizadores..."
        $DB_QUERY "SELECT Id, Username, Email, Role, CustomerId, EmployeeId FROM [MechaSoftCS].[Users] WHERE IsDeleted = 0"
        ;;
    customers|clientes)
        echo "📋 Listando clientes..."
        $DB_QUERY "SELECT TOP 20 Id, FirstName + ' ' + LastName AS Nome, Email, Phone, Type FROM [MechaSoftCS].[Customer] WHERE IsDeleted = 0"
        ;;
    employees|funcionarios)
        echo "📋 Listando funcionários..."
        $DB_QUERY "SELECT TOP 20 Id, FirstName + ' ' + LastName AS Nome, Email, Phone, Role FROM [MechaSoftCS].[Employee] WHERE IsDeleted = 0"
        ;;
    orders|ordens)
        echo "📋 Listando ordens de serviço..."
        $DB_QUERY "SELECT TOP 20 Id, Status, Priority, CreatedAt FROM [MechaSoftCS].[ServiceOrder] WHERE IsDeleted = 0 ORDER BY CreatedAt DESC"
        ;;
    tables|tabelas)
        echo "📋 Listando tabelas..."
        $DB_QUERY "SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'MechaSoftCS' ORDER BY TABLE_NAME"
        ;;
    clear-customer)
        if [ -z "$2" ]; then
            echo "❌ Forneça o ID do utilizador"
            echo "Uso: ./db-queries.sh clear-customer <user-id>"
            exit 1
        fi
        echo "🔄 Limpando perfil de cliente do utilizador $2..."
        # Get CustomerId
        CUSTOMER_ID=$($DB_QUERY "SELECT CAST(CustomerId AS VARCHAR(36)) FROM [MechaSoftCS].[Users] WHERE Id = '$2'" | grep -E '^[0-9A-F]{8}-' | xargs)
        
        if [ -n "$CUSTOMER_ID" ] && [ "$CUSTOMER_ID" != "NULL" ]; then
            echo "📦 Deletando cliente $CUSTOMER_ID..."
            $DB_QUERY "SET QUOTED_IDENTIFIER ON; DELETE FROM [MechaSoftCS].[Customer] WHERE Id = '$CUSTOMER_ID'"
        fi
        
        echo "🔄 Removendo CustomerId do utilizador..."
        $DB_QUERY "SET QUOTED_IDENTIFIER ON; UPDATE [MechaSoftCS].[Users] SET CustomerId = NULL WHERE Id = '$2'"
        echo "✅ Perfil de cliente limpo com sucesso!"
        ;;
    clear-employee)
        if [ -z "$2" ]; then
            echo "❌ Forneça o ID do utilizador"
            echo "Uso: ./db-queries.sh clear-employee <user-id>"
            exit 1
        fi
        echo "🔄 Removendo EmployeeId do utilizador $2..."
        $DB_QUERY "SET QUOTED_IDENTIFIER ON; UPDATE [MechaSoftCS].[Users] SET EmployeeId = NULL WHERE Id = '$2'"
        echo "✅ EmployeeId removido!"
        ;;
    *)
        echo "🗄️  MechaSoft - Queries SQL Úteis"
        echo ""
        echo "Comandos disponíveis:"
        echo "  users, utilizadores     - Listar utilizadores"
        echo "  customers, clientes     - Listar clientes"
        echo "  employees, funcionarios - Listar funcionários"
        echo "  orders, ordens          - Listar ordens de serviço"
        echo "  tables, tabelas         - Listar tabelas"
        echo "  clear-customer <id>     - Remover CustomerId de um utilizador"
        echo "  clear-employee <id>     - Remover EmployeeId de um utilizador"
        echo ""
        echo "Exemplos:"
        echo "  ./db-queries.sh users"
        echo "  ./db-queries.sh clientes"
        echo "  ./db-queries.sh clear-customer a427c596-2857-41ff-a964-e1c934fbed48"
        ;;
esac

