# MechaSoft App - Stop Script (Windows PowerShell)
Write-Host "🛑 Parando MechaSoft App..." -ForegroundColor Red

# Parar processos .NET (Backend)
Write-Host "`n🔍 Procurando processos Backend..." -ForegroundColor Cyan
$dotnetProcesses = Get-Process | Where-Object {$_.ProcessName -like "*MechaSoft.WebAPI*" -or ($_.ProcessName -eq "dotnet" -and $_.MainWindowTitle -like "*MechaSoft*")}

if ($dotnetProcesses) {
    $dotnetProcesses | Stop-Process -Force
    Write-Host "✅ Backend parado ($($dotnetProcesses.Count) processo(s))" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Nenhum processo Backend encontrado" -ForegroundColor Gray
}

# Parar processos Node (Frontend)
Write-Host "`n🔍 Procurando processos Frontend..." -ForegroundColor Cyan
$nodeProcesses = Get-Process | Where-Object {$_.ProcessName -eq "node" -and $_.CommandLine -like "*ng serve*"}

if ($nodeProcesses) {
    $nodeProcesses | Stop-Process -Force
    Write-Host "✅ Frontend parado ($($nodeProcesses.Count) processo(s))" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Nenhum processo Frontend encontrado" -ForegroundColor Gray
}

Write-Host "`n✅ MechaSoft App parado!" -ForegroundColor Green

