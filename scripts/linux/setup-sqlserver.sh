#!/bin/bash
echo "🚀 MechaSoft App - Configurando SQL Server no Ubuntu"
echo "=================================================="

# Verificar se está no diretório correto
if [ ! -f "MechaSoft.sln" ]; then
    echo "❌ Execute este script no diretório raiz do projeto MechaSoftApp"
    exit 1
fi

echo "✅ Verificando dependências..."

# Instalar Docker se não estiver instalado
if ! command -v docker &> /dev/null; then
    echo "⚠️  Docker não encontrado. Instalando Docker..."
    sudo apt update
    sudo apt install -y docker.io
    sudo systemctl start docker
    sudo systemctl enable docker
    sudo usermod -aG docker $USER
    newgrp docker
    echo "✅ Docker instalado com sucesso!"
else
    echo "✅ Docker já está instalado"
fi

# Verificar se .NET está instalado
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK não encontrado. Instale o .NET 8.0 SDK primeiro:"
    echo "sudo snap install dotnet-sdk --classic"
    exit 1
else
    echo "✅ .NET SDK encontrado"
fi

# Parar container SQL Server se estiver rodando
echo "🔄 Parando container SQL Server existente (se houver)..."
docker stop mechasoft-sqlserver 2>/dev/null || true
docker rm mechasoft-sqlserver 2>/dev/null || true

# Configurar senha do SQL Server
SQL_PASSWORD="CHANGE_ME_PASSWORD"
echo "🔧 Configurando SQL Server com senha: $SQL_PASSWORD"

# Executar SQL Server 2022 via Docker
echo "🚀 Iniciando SQL Server 2022..."
docker run -e "ACCEPT_EULA=Y" \
           -e "SA_PASSWORD=$SQL_PASSWORD" \
           -e "MSSQL_PID=Express" \
           -p 1433:1433 \
           --name mechasoft-sqlserver \
           --hostname mechasoft-sqlserver \
           -d mcr.microsoft.com/mssql/server:2022-latest

# Aguardar SQL Server inicializar
echo "⏳ Aguardando SQL Server inicializar (30 segundos)..."
sleep 30

# Verificar se container está rodando
if ! docker ps | grep -q mechasoft-sqlserver; then
    echo "❌ Falha ao iniciar SQL Server. Verificando logs..."
    docker logs mechasoft-sqlserver
    exit 1
fi

echo "✅ SQL Server iniciado com sucesso!"

# Atualizar connection string no appsettings.Development.json
echo "🔧 Atualizando connection string..."

# Backup do arquivo original
cp MechaSoft.WebAPI/appsettings.Development.json MechaSoft.WebAPI/appsettings.Development.json.backup

# Nova connection string para Ubuntu/Linux
NEW_CONNECTION_STRING="Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=$SQL_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"

# Atualizar arquivo
cat > MechaSoft.WebAPI/appsettings.Development.json << EOF
{
  "ConnectionStrings": {
    "MechaSoftCS": "$NEW_CONNECTION_STRING"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JwtSettings": {
    "Key": "CHANGE_ME_WITH_A_64_BYTE_MINIMUM_SECRET_KEY",
    "Issuer": "https://www.mechasoft.pt",
    "Audience": "https://www.mechasoft.pt/api",
    "ExpirationInMinutes": 60
  }
}
