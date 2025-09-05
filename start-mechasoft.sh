#!/bin/bash
echo "🚀 Iniciando MechaSoft App..."

# Verificar se SQL Server está rodando
if ! docker ps | grep -q mechasoft-sqlserver; then
    echo "⚠️  SQL Server não está rodando. Iniciando..."
    docker start mechasoft-sqlserver
    sleep 10
fi

# Iniciar WebAPI
echo "🌐 Iniciando WebAPI..."
cd MechaSoft.WebAPI
dotnet run &
WEBAPI_PID=$!

# Aguardar um pouco
sleep 5

# Iniciar Angular (se existir)
if [ -d "../Presentation/MechaSoft.Angular" ]; then
    echo "🅰️  Iniciando Angular..."
    cd ../Presentation/MechaSoft.Angular
    npm start &
    ANGULAR_PID=$!
fi

echo "✅ MechaSoft App iniciado!"
echo "🌐 WebAPI: http://localhost:5000"
echo "️  Angular: http://localhost:4200"
echo ""
echo "Para parar: kill $WEBAPI_PID $ANGULAR_PID"

# Manter script rodando
wait
