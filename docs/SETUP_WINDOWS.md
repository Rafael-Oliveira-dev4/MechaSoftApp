# 🪟 MechaSoft - Configuração Windows

**Plataforma:** Windows 10/11  
**Ambiente:** PowerShell

---

## 📋 PRÉ-REQUISITOS

### 1. .NET SDK 8.0
```powershell
# Baixar e instalar de:
# https://dotnet.microsoft.com/download/dotnet/8.0

# Verificar instalação
dotnet --version
```

### 2. Node.js e npm
```powershell
# Baixar e instalar de:
# https://nodejs.org/ (versão LTS)

# Verificar instalação
node --version
npm --version
```

### 3. SQL Server
Escolha uma opção:

#### Opção A: SQL Server Express (Recomendado para Windows)
```powershell
# Baixar de:
# https://www.microsoft.com/sql-server/sql-server-downloads

# Durante instalação:
# - Escolher "Basic" ou "Custom"
# - Anotar o nome da instância (ex: SQLEXPRESS)
# - Configurar autenticação mista (Windows + SQL Server)
```

#### Opção B: Docker Desktop (Opcional)
```powershell
# Baixar e instalar:
# https://www.docker.com/products/docker-desktop

# Após instalação, executar:
docker run -e "ACCEPT_EULA=Y" `
           -e "SA_PASSWORD=CHANGE_ME_PASSWORD" `
           -e "MSSQL_PID=Express" `
           -p 1433:1433 `
           --name mechasoft-sqlserver `
           -d mcr.microsoft.com/mssql/server:2022-latest
```

---

## 🔧 CONFIGURAÇÃO DA CONNECTION STRING

### Para SQL Server Express (Local)
Editar `MechaSoft.WebAPI/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost\\SQLEXPRESS;Database=DV_RO_MechaSoft;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### Para SQL Server em Docker
```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 🚀 INSTALAÇÃO E CONFIGURAÇÃO

### 1. Clonar/Abrir o Projeto
```powershell
cd C:\Projects
git clone [URL_DO_REPOSITORIO]
cd MechaSoftApp
```

### 2. Instalar Entity Framework Tools
```powershell
dotnet tool install --global dotnet-ef
```

### 3. Restaurar Pacotes .NET
```powershell
dotnet restore
```

### 4. Compilar Backend
```powershell
dotnet build --configuration Release
```

### 5. Instalar Dependências Angular
```powershell
cd Presentation\MechaSoft.Angular
npm install
cd ..\..
```

### 6. Executar Migrações
```powershell
dotnet ef database update --project .\MechaSoft.Data\ --startup-project .\MechaSoft.WebAPI\
```

---

## ▶️ EXECUTAR A APLICAÇÃO

### Opção 1: Scripts PowerShell

**Iniciar aplicação:**
```powershell
.\start-mechasoft.ps1
```

**Parar aplicação:**
```powershell
.\stop-mechasoft.ps1
```

### Opção 2: Comandos Manuais

**Backend:**
```powershell
# Terminal 1
cd MechaSoft.WebAPI
dotnet run --urls "http://localhost:5039"
```

**Frontend:**
```powershell
# Terminal 2
cd Presentation\MechaSoft.Angular
npm start -- --port 4300
```

---

## 🛠️ SCRIPTS POWERSHELL

### start-mechasoft.ps1
```powershell
Write-Host "🚀 Iniciando MechaSoft App..." -ForegroundColor Green

# Iniciar Backend
Write-Host "🌐 Iniciando Backend..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\MechaSoft.WebAPI'; dotnet run --urls 'http://localhost:5039'"

# Aguardar
Start-Sleep -Seconds 5

# Iniciar Frontend
Write-Host "🅰️  Iniciando Frontend..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\Presentation\MechaSoft.Angular'; npm start -- --port 4300"

Write-Host "`n✅ Aplicação iniciada!" -ForegroundColor Green
Write-Host "🌐 Backend: http://localhost:5039" -ForegroundColor Yellow
Write-Host "🌐 Swagger: http://localhost:5039/swagger" -ForegroundColor Yellow
Write-Host "🅰️  Frontend: http://localhost:4300" -ForegroundColor Yellow
```

### stop-mechasoft.ps1
```powershell
Write-Host "🛑 Parando MechaSoft App..." -ForegroundColor Red

# Parar processos .NET
Get-Process | Where-Object {$_.ProcessName -like "*MechaSoft.WebAPI*"} | Stop-Process -Force
Write-Host "✅ Backend parado" -ForegroundColor Green

# Parar processos Node
Get-Process | Where-Object {$_.ProcessName -like "*node*" -and $_.CommandLine -like "*ng serve*"} | Stop-Process -Force
Write-Host "✅ Frontend parado" -ForegroundColor Green

Write-Host "`n✅ Aplicação parada!" -ForegroundColor Green
```

### setup-db.ps1
```powershell
Write-Host "🔧 Configurando Base de Dados..." -ForegroundColor Cyan

# Verificar se SQL Server está acessível
$connectionTest = Test-NetConnection -ComputerName localhost -Port 1433

if ($connectionTest.TcpTestSucceeded) {
    Write-Host "✅ SQL Server está acessível" -ForegroundColor Green
    
    # Executar migrações
    Write-Host "🚀 Executando migrações..." -ForegroundColor Cyan
    dotnet ef database update --project .\MechaSoft.Data\ --startup-project .\MechaSoft.WebAPI\
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`n✅ Base de dados configurada com sucesso!" -ForegroundColor Green
    } else {
        Write-Host "`n❌ Erro ao executar migrações" -ForegroundColor Red
    }
} else {
    Write-Host "❌ SQL Server não está acessível na porta 1433" -ForegroundColor Red
    Write-Host "Verifique se o SQL Server está rodando" -ForegroundColor Yellow
}
```

---

## 🧪 TESTAR A APLICAÇÃO

### Health Check
```powershell
Invoke-WebRequest -Uri http://localhost:5039/health
```

### Swagger UI
Abrir no navegador: http://localhost:5039/swagger

### Frontend
Abrir no navegador: http://localhost:4300

---

## 🔍 TROUBLESHOOTING (Windows)

### Porta em uso
```powershell
# Ver o que está usando a porta
netstat -ano | findstr :5039
netstat -ano | findstr :4300

# Matar processo por PID
taskkill /PID [PID] /F
```

### SQL Server não conecta
```powershell
# Verificar se serviço está rodando
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# Iniciar serviço
Start-Service MSSQL$SQLEXPRESS
```

### Erros de compilação
```powershell
# Limpar e recompilar
dotnet clean
dotnet restore
dotnet build
```

### Reset completo
```powershell
# Parar tudo
.\stop-mechasoft.ps1

# Limpar
dotnet clean
Remove-Item .\Presentation\MechaSoft.Angular\node_modules -Recurse -Force

# Reinstalar
dotnet restore
cd Presentation\MechaSoft.Angular
npm install
cd ..\..

# Configurar DB
.\setup-db.ps1

# Iniciar
.\start-mechasoft.ps1
```

---

## 📝 VARIÁVEIS DE AMBIENTE (Opcional)

### Criar ficheiro .env (não commitar!)
```env
ConnectionStrings__MechaSoftCS=Server=localhost\SQLEXPRESS;Database=DV_RO_MechaSoft;...
JwtSettings__Key=sua-chave-secreta-aqui
```

### Usar em PowerShell
```powershell
# Carregar variáveis
Get-Content .env | ForEach-Object {
    $name, $value = $_.split('=')
    Set-Content env:\$name $value
}
```

---

## 🎯 DESENVOLVIMENTO NO VISUAL STUDIO

### Abrir Solução
```powershell
start MechaSoft.sln
```

### Configurar Startup Projects
1. Botão direito na solução → Properties
2. Selecionar "Multiple startup projects"
3. Configurar:
   - MechaSoft.WebAPI → Start
   - (Frontend separado no terminal)

### Debug
- F5 para iniciar com debug
- Ctrl+F5 para iniciar sem debug

---

## 📦 BUILD PARA PRODUÇÃO

### Backend
```powershell
dotnet publish -c Release -o ./publish
```

### Frontend
```powershell
cd Presentation\MechaSoft.Angular
npm run build
# Output em: dist\MechaSoft.Angular\
```

---

## 🔐 CREDENCIAIS (Desenvolvimento)

### SQL Server Express
- **Autenticação:** Windows Authentication (Trusted_Connection=True)
- **OU** SQL Server Authentication (sa / sua-senha)

### SQL Server Docker
- **User:** sa
- **Password:** CHANGE_ME_PASSWORD

---

**Preparado para Windows 10/11**  
**PowerShell 5.1+ ou PowerShell Core 7+**

