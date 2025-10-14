# MechaSoft App - Start Script (Windows PowerShell)
Write-Host "🚀 Iniciando MechaSoft App..." -ForegroundColor Green

# Verificar se SQL Server está acessível
Write-Host "`n🔍 Verificando SQL Server..." -ForegroundColor Cyan
$connectionTest = Test-NetConnection -ComputerName localhost -Port 1433 -WarningAction SilentlyContinue

if ($connectionTest.TcpTestSucceeded) {
    Write-Host "✅ SQL Server está acessível" -ForegroundColor Green
} else {
    Write-Host "⚠️  SQL Server não está acessível na porta 1433" -ForegroundColor Yellow
    Write-Host "   A aplicação iniciará mas operações de BD falharão" -ForegroundColor Yellow
}

# Iniciar Backend
Write-Host "`n🌐 Iniciando Backend WebAPI..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\MechaSoft.WebAPI'; Write-Host 'Backend WebAPI - Porta 5039' -ForegroundColor Green; dotnet run --urls 'http://localhost:5039'"

# Aguardar backend inicializar
Write-Host "   Aguardando backend inicializar..." -ForegroundColor Gray
Start-Sleep -Seconds 5

# Iniciar Frontend
Write-Host "`n🅰️  Iniciando Frontend Angular..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\Presentation\MechaSoft.Angular'; Write-Host 'Frontend Angular - Porta 4300' -ForegroundColor Green; npm start -- --port 4300"

Write-Host "`n✅ MechaSoft App iniciado!" -ForegroundColor Green
Write-Host "`n📊 URLs de Acesso:" -ForegroundColor Yellow
Write-Host "   🌐 Backend:  http://localhost:5039" -ForegroundColor Cyan
Write-Host "   🌐 Swagger:  http://localhost:5039/swagger" -ForegroundColor Cyan
Write-Host "   🅰️  Frontend: http://localhost:4300" -ForegroundColor Cyan
Write-Host "`n💡 Dica: Aguarde alguns segundos para as aplicações iniciarem completamente" -ForegroundColor Gray
Write-Host "💡 Para parar: Execute .\stop-mechasoft.ps1" -ForegroundColor Gray

