# MechaSoft App - Database Setup Script (Windows PowerShell)
Write-Host "🔧 MechaSoft - Configuração da Base de Dados" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan

# Verificar se SQL Server está acessível
Write-Host "`n📊 Verificando conexão com SQL Server..." -ForegroundColor Cyan
$connectionTest = Test-NetConnection -ComputerName localhost -Port 1433 -WarningAction SilentlyContinue

if (-not $connectionTest.TcpTestSucceeded) {
    Write-Host "❌ SQL Server não está acessível na porta 1433" -ForegroundColor Red
    Write-Host "`n💡 Soluções:" -ForegroundColor Yellow
    Write-Host "   1. Se usar SQL Server Express, verifique o serviço:" -ForegroundColor Gray
    Write-Host "      Get-Service | Where-Object {`$_.Name -like '*SQL*'}" -ForegroundColor Gray
    Write-Host "   2. Se usar Docker, verifique o container:" -ForegroundColor Gray
    Write-Host "      docker ps | findstr mechasoft" -ForegroundColor Gray
    exit 1
}

Write-Host "✅ SQL Server está acessível!" -ForegroundColor Green

# Verificar se dotnet-ef está instalado
Write-Host "`n🔍 Verificando Entity Framework Tools..." -ForegroundColor Cyan
$efVersion = dotnet ef --version 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "⚠️  Entity Framework Tools não instalado" -ForegroundColor Yellow
    Write-Host "📦 Instalando dotnet-ef..." -ForegroundColor Cyan
    dotnet tool install --global dotnet-ef
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Entity Framework Tools instalado!" -ForegroundColor Green
    } else {
        Write-Host "❌ Erro ao instalar Entity Framework Tools" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "✅ Entity Framework Tools instalado ($efVersion)" -ForegroundColor Green
}

# Executar migrações
Write-Host "`n🚀 Executando migrações do Entity Framework..." -ForegroundColor Cyan
Write-Host "   Aguarde, isto pode demorar alguns segundos..." -ForegroundColor Gray

dotnet ef database update --project .\MechaSoft.Data\ --startup-project .\MechaSoft.WebAPI\

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✅ Base de dados configurada com sucesso!" -ForegroundColor Green
    Write-Host "`n🎯 Próximos passos:" -ForegroundColor Yellow
    Write-Host "   1. Iniciar aplicação: .\start-mechasoft.ps1" -ForegroundColor Cyan
    Write-Host "   2. Aceder Swagger:    http://localhost:5039/swagger" -ForegroundColor Cyan
    Write-Host "   3. Aceder Frontend:   http://localhost:4300" -ForegroundColor Cyan
} else {
    Write-Host "`n❌ Erro ao executar migrações" -ForegroundColor Red
    Write-Host "`n📋 Verifique:" -ForegroundColor Yellow
    Write-Host "   - Connection string em appsettings.Development.json" -ForegroundColor Gray
    Write-Host "   - Se SQL Server está rodando" -ForegroundColor Gray
    Write-Host "   - Logs de erro acima" -ForegroundColor Gray
    exit 1
}

