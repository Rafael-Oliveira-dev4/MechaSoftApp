#!/bin/bash
echo "🔧 MechaSoft - Configuração da Base de Dados"
echo "=============================================="

# Verificar se container existe
echo "📊 Verificando container SQL Server..."
if sudo docker ps -a | grep -q mechasoft-sqlserver; then
    echo "✅ Container encontrado"
    
    # Verificar se está rodando
    if sudo docker ps | grep -q mechasoft-sqlserver; then
        echo "✅ SQL Server já está rodando"
    else
        echo "🔄 Iniciando SQL Server..."
        sudo docker start mechasoft-sqlserver
    fi
else
    echo "❌ Container não encontrado. Execute: sudo ./setup-sqlserver.sh"
    exit 1
fi

# Aguardar SQL Server inicializar
echo "⏳ Aguardando SQL Server inicializar (60 segundos)..."
sleep 60

# Verificar logs
echo "📋 Verificando se SQL Server está pronto..."
if sudo docker logs mechasoft-sqlserver 2>&1 | grep -q "SQL Server is now ready for client connections"; then
    echo "✅ SQL Server está pronto!"
else
    echo "⚠️  SQL Server ainda está inicializando. Aguarde mais 30 segundos..."
    sleep 30
fi

# Executar migrações
echo "🚀 Executando migrações do Entity Framework..."
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI

if [ $? -eq 0 ]; then
    echo "✅ Base de dados configurada com sucesso!"
    echo ""
    echo "🎯 Próximos passos:"
    echo "   - Aceder ao Swagger: http://localhost:5039/swagger"
    echo "   - Aceder ao Frontend: http://localhost:4300"
    echo ""
else
    echo "❌ Erro ao executar migrações"
    echo "📋 Verificar logs do SQL Server:"
    echo "   sudo docker logs mechasoft-sqlserver"
fi

