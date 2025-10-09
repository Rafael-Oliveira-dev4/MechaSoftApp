# 🪟 Scripts Windows (PowerShell)

Scripts para gerir a aplicação MechaSoft no Windows.

---

## 📜 Scripts Disponíveis

### `start.ps1` - Iniciar Aplicação
Inicia o backend (.NET) e frontend (Angular).

```powershell
.\start.ps1
```

**O que faz:**
- ✅ Verifica se SQL Server está acessível
- ▶️ Inicia Backend WebAPI na porta 5039
- ▶️ Inicia Frontend Angular na porta 4300
- 📊 Mostra URLs de acesso

---

### `stop.ps1` - Parar Aplicação
Para todos os processos do backend e frontend.

```powershell
.\stop.ps1
```

**O que faz:**
- 🔍 Procura processos MechaSoft.WebAPI
- 🔍 Procura processos Node.js (ng serve)
- ⏹️ Para todos os processos encontrados

---

### `setup-db.ps1` - Configurar Base de Dados
Executa migrações do Entity Framework.

```powershell
.\setup-db.ps1
```

**O que faz:**
- 🔍 Verifica se SQL Server está acessível (porta 1433)
- 🔍 Verifica se dotnet-ef está instalado
- 📦 Instala dotnet-ef se necessário
- 🚀 Executa migrações do Entity Framework
- ✅ Confirma sucesso

---

## 🚀 Sequência de Uso (Primeira Vez)

```powershell
# 1. Navegar para a pasta de scripts
cd scripts\windows

# 2. Configurar base de dados (primeira vez)
.\setup-db.ps1

# 3. Voltar à raiz e iniciar aplicação
cd ..\..
.\scripts\windows\start.ps1

# 4. Aceder aplicação
# - Backend:  http://localhost:5039
# - Swagger:  http://localhost:5039/swagger
# - Frontend: http://localhost:4300

# 5. Parar quando terminar
.\scripts\windows\stop.ps1
```

---

## 📋 Pré-requisitos

- ✅ .NET SDK 8.0+
- ✅ Node.js 18+ e npm
- ✅ SQL Server (Express ou Docker)
- ✅ PowerShell 5.1+ ou PowerShell Core 7+

---

## 🔧 Configuração SQL Server

### SQL Server Express (Recomendado para Windows)

Connection string em `MechaSoft.WebAPI\appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost\\SQLEXPRESS;Database=DV_RO_MechaSoft;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### Docker (Alternativa)

```powershell
docker run -e "ACCEPT_EULA=Y" `
           -e "SA_PASSWORD=CHANGE_ME_PASSWORD" `
           -e "MSSQL_PID=Express" `
           -p 1433:1433 `
           --name mechasoft-sqlserver `
           -d mcr.microsoft.com/mssql/server:2022-latest
```

Connection string:
```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 🆘 Resolução de Problemas

### Script não executa
```powershell
# Habilitar execução de scripts
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### Porta em uso
```powershell
# Ver o que está usando a porta
netstat -ano | findstr :5039

# Matar processo
taskkill /PID [PID] /F
```

### SQL Server não conecta
```powershell
# Verificar serviço
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# Iniciar serviço
Start-Service MSSQL$SQLEXPRESS
```

---

📖 **Ver documentação completa:** [docs/SETUP_WINDOWS.md](../../docs/SETUP_WINDOWS.md)

