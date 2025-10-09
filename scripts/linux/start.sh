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
# ORIGINAL: dotnet run
# SUGESTÃO: Usar porta HTTP 5039 conforme configuração do projeto (README.md)
dotnet run --urls "http://localhost:5039" &
WEBAPI_PID=$!

# Aguardar um pouco
sleep 5

# Iniciar Angular (se existir)
if [ -d "../Presentation/MechaSoft.Angular" ]; then
    echo "🅰️  Iniciando Angular..."
    cd ../Presentation/MechaSoft.Angular
    # ORIGINAL: npm start (porta padrão 4200)
    # SUGESTÃO: Usar porta 4300 conforme recomendado no README.md linha 69
    npx ng serve --port 4300 --open=false &
    ANGULAR_PID=$!
fi

echo "✅ MechaSoft App iniciado!"
# ORIGINAL: echo "🌐 WebAPI: http://localhost:5000"
# SUGESTÃO: Portas corretas conforme configuração
echo "🌐 WebAPI: http://localhost:5039"
echo "🌐 Swagger: http://localhost:5039/swagger"
# ORIGINAL: echo "️  Angular: http://localhost:4200"
# SUGESTÃO: Porta 4300
echo "🅰️  Angular: http://localhost:4300"
echo ""
echo "Para parar: kill $WEBAPI_PID $ANGULAR_PID"

# Manter script rodando
wait
