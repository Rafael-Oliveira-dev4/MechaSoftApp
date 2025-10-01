# Script de teste de integracao MechaSoft
# Windows PowerShell

Write-Host "=== TESTE DE INTEGRACAO MECHASOFT ===" -ForegroundColor Cyan
Write-Host ""

# 1. Verificar SQL Server
Write-Host "1. Verificando SQL Server..." -ForegroundColor Yellow
try {
    $sqlTest = sqlcmd -S "localhost\SQLEXPRESS" -Q "SELECT 1" -C 2>&1
    Write-Host "   ✅ SQL Server OK" -ForegroundColor Green
} catch {
    Write-Host "   ❌ SQL Server nao esta rodando!" -ForegroundColor Red
    exit 1
}

# 2. Compilar Backend
Write-Host ""
Write-Host "2. Compilando Backend..." -ForegroundColor Yellow
Set-Location "MechaSoft.WebAPI"

# Verificar se dotnet esta disponivel
if (!(Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Host "   ❌ .NET SDK nao encontrado!" -ForegroundColor Red
    Write-Host "   Instale: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    Set-Location ..
    exit 1
}

$buildResult = dotnet build --nologo 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✅ Backend compilado com sucesso" -ForegroundColor Green
} else {
    Write-Host "   ❌ Erro ao compilar backend" -ForegroundColor Red
    Write-Host $buildResult
    Set-Location ..
    exit 1
}

# 3. Iniciar Backend (background)
Write-Host ""
Write-Host "3. Iniciando Backend..." -ForegroundColor Yellow
$backendJob = Start-Job -ScriptBlock {
    Set-Location $using:PWD
    dotnet run --no-build --launch-profile https
}
Start-Sleep -Seconds 5

if ($backendJob.State -eq "Running") {
    Write-Host "   ✅ Backend iniciado (HTTPS: 7277, HTTP: 5039)" -ForegroundColor Green
} else {
    Write-Host "   ❌ Erro ao iniciar backend" -ForegroundColor Red
    Receive-Job $backendJob
    exit 1
}

Set-Location ..

# 4. Verificar node_modules
Write-Host ""
Write-Host "4. Verificando Frontend..." -ForegroundColor Yellow
Set-Location "Presentation\MechaSoft.Angular"

if (!(Test-Path "node_modules")) {
    Write-Host "   Instalando dependencias..." -ForegroundColor Yellow
    npm install
}

# 5. Iniciar Frontend (background)
Write-Host ""
Write-Host "5. Iniciando Frontend..." -ForegroundColor Yellow
$frontendJob = Start-Job -ScriptBlock {
    Set-Location $using:PWD
    npm start
}
Start-Sleep -Seconds 10

if ($frontendJob.State -eq "Running") {
    Write-Host "   ✅ Frontend iniciado (http://localhost:4200)" -ForegroundColor Green
} else {
    Write-Host "   ❌ Erro ao iniciar frontend" -ForegroundColor Red
}

Set-Location ..\..

# 6. Resumo
Write-Host ""
Write-Host "=== SERVICOS INICIADOS ===" -ForegroundColor Green
Write-Host ""
Write-Host "📊 Backend (Swagger):" -ForegroundColor Cyan
Write-Host "   https://localhost:7277/swagger" -ForegroundColor White
Write-Host ""
Write-Host "🎨 Frontend (App):" -ForegroundColor Cyan
Write-Host "   http://localhost:4200" -ForegroundColor White
Write-Host ""
Write-Host "🔑 Para testar, crie um usuario:" -ForegroundColor Yellow
Write-Host "   POST https://localhost:7277/api/accounts/register" -ForegroundColor White
Write-Host "   Body: {username, email, password, confirmPassword, role: 'Owner'}" -ForegroundColor Gray
Write-Host ""
Write-Host "Para parar os servicos:" -ForegroundColor Yellow
Write-Host "   Stop-Job $($backendJob.Id)" -ForegroundColor White
Write-Host "   Stop-Job $($frontendJob.Id)" -ForegroundColor White
Write-Host ""
Write-Host "Pressione Ctrl+C para parar tudo e limpar." -ForegroundColor Red
Write-Host ""

# Manter script rodando
try {
    while ($true) {
        Start-Sleep -Seconds 5
        
        # Verificar se jobs ainda estao rodando
        if ($backendJob.State -ne "Running") {
            Write-Host "Backend parou!" -ForegroundColor Red
            break
        }
        if ($frontendJob.State -ne "Running") {
            Write-Host "Frontend parou!" -ForegroundColor Red
            break
        }
    }
} finally {
    Write-Host ""
    Write-Host "Parando servicos..." -ForegroundColor Yellow
    Stop-Job $backendJob -ErrorAction SilentlyContinue
    Stop-Job $frontendJob -ErrorAction SilentlyContinue
    Remove-Job $backendJob -ErrorAction SilentlyContinue
    Remove-Job $frontendJob -ErrorAction SilentlyContinue
    Write-Host "Servicos parados." -ForegroundColor Green
}

