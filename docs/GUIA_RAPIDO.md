# ⚡ MechaSoft - Guia Rápido

**Última Atualização:** 09/10/2025

---

## 📁 ORGANIZAÇÃO DA DOCUMENTAÇÃO

### 📖 Documentação por Plataforma

| Plataforma | Ficheiro | Conteúdo |
|------------|----------|----------|
| 🪟 **Windows** | [SETUP_WINDOWS.md](SETUP_WINDOWS.md) | Configuração completa para Windows 10/11 |
| 🐧 **Linux** | [SETUP_LINUX.md](SETUP_LINUX.md) | Configuração completa para Linux (Debian/Ubuntu) |
| 📊 **Geral** | [STATUS_PROJETO.md](STATUS_PROJETO.md) | Estado atual do projeto |
| 🎯 **Geral** | [PROXIMOS_PASSOS.md](PROXIMOS_PASSOS.md) | Comandos úteis e próximos passos |
| 📝 **Geral** | [README.md](README.md) | Visão geral do projeto |
| 🔧 **Geral** | [SETUP.md](SETUP.md) | Configuração detalhada original |

---

## 🛠️ SCRIPTS POR PLATAFORMA

### Windows (PowerShell)

| Script | Descrição | Comando |
|--------|-----------|---------|
| `start-mechasoft.ps1` | Inicia backend e frontend | `.\start-mechasoft.ps1` |
| `stop-mechasoft.ps1` | Para backend e frontend | `.\stop-mechasoft.ps1` |
| `setup-db.ps1` | Configura base de dados | `.\setup-db.ps1` |

### Linux (Bash)

| Script | Descrição | Comando |
|--------|-----------|---------|
| `start-mechasoft.sh` | Inicia backend e frontend | `./start-mechasoft.sh` |
| `stop-mechasoft.sh` | Para backend e frontend | `./stop-mechasoft.sh` |
| `setup-sqlserver.sh` | Configura SQL Server Docker | `sudo ./setup-sqlserver.sh` |
| `setup-db.sh` | Executa migrações | `./setup-db.sh` |

---

## 🎯 INÍCIO RÁPIDO POR PLATAFORMA

### 🪟 Windows

```powershell
# 1. Configurar base de dados (primeira vez)
.\setup-db.ps1

# 2. Iniciar aplicação
.\start-mechasoft.ps1

# 3. Aceder aplicação
# - Backend:  http://localhost:5039
# - Swagger:  http://localhost:5039/swagger
# - Frontend: http://localhost:4300

# 4. Parar aplicação
.\stop-mechasoft.ps1
```

### 🐧 Linux

```bash
# 1. Configurar SQL Server (primeira vez)
sudo ./setup-sqlserver.sh

# 2. Configurar base de dados
./setup-db.sh

# 3. Iniciar aplicação
./start-mechasoft.sh

# 4. Aceder aplicação
# - Backend:  http://localhost:5039
# - Swagger:  http://localhost:5039/swagger
# - Frontend: http://localhost:4300

# 5. Parar aplicação
./stop-mechasoft.sh
```

---

## 🔧 CONNECTION STRINGS POR PLATAFORMA

### Windows - SQL Server Express (Recomendado)

```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost\\SQLEXPRESS;Database=DV_RO_MechaSoft;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### Linux - SQL Server Docker (Recomendado)

```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Universal - SQL Server Docker (Funciona em ambos)

```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 🌐 URLs DA APLICAÇÃO

| Serviço | URL | Descrição |
|---------|-----|-----------|
| Backend API | http://localhost:5039 | API REST |
| Health Check | http://localhost:5039/health | Status do backend |
| Swagger UI | http://localhost:5039/swagger | Documentação interativa da API |
| Frontend Angular | http://localhost:4300 | Interface de utilizador |

---

## 📦 COMANDOS COMUNS

### .NET

```bash
# Restaurar pacotes
dotnet restore

# Compilar
dotnet build

# Executar
dotnet run --project MechaSoft.WebAPI

# Executar com URL específica
dotnet run --project MechaSoft.WebAPI --urls "http://localhost:5039"

# Limpar
dotnet clean
```

### Entity Framework

```bash
# Windows
dotnet ef database update --project .\MechaSoft.Data\ --startup-project .\MechaSoft.WebAPI\
dotnet ef migrations add NomeMigracao --project .\MechaSoft.Data\ --startup-project .\MechaSoft.WebAPI\

# Linux
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
dotnet ef migrations add NomeMigracao --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

### Angular

```bash
# Windows
cd Presentation\MechaSoft.Angular
npm install
npm start -- --port 4300

# Linux
cd Presentation/MechaSoft.Angular
npm install
npm start -- --port 4300
```

---

## 🔍 VERIFICAR PROBLEMAS

### Windows

```powershell
# Verificar SQL Server
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# Verificar portas em uso
netstat -ano | findstr :5039
netstat -ano | findstr :4300

# Verificar processos
Get-Process | Where-Object {$_.ProcessName -like "*MechaSoft*"}
Get-Process | Where-Object {$_.ProcessName -eq "node"}

# Testar backend
Invoke-WebRequest -Uri http://localhost:5039/health
```

### Linux

```bash
# Verificar SQL Server (Docker)
sudo docker ps | grep mechasoft-sqlserver

# Verificar portas em uso
ss -tulpn | grep -E ':(5039|4300)'
lsof -i :5039

# Verificar processos
ps aux | grep -E "(MechaSoft.WebAP|ng serve)" | grep -v grep

# Testar backend
curl http://localhost:5039/health
```

---

## 🆘 PROBLEMAS COMUNS

### ❌ "Porta já em uso"

**Windows:**
```powershell
netstat -ano | findstr :5039
taskkill /PID [PID] /F
```

**Linux:**
```bash
lsof -i :5039
kill -9 [PID]
```

### ❌ "SQL Server não conecta"

**Windows:**
```powershell
# Verificar serviço
Get-Service MSSQL$SQLEXPRESS

# Iniciar se parado
Start-Service MSSQL$SQLEXPRESS
```

**Linux:**
```bash
# Verificar container
sudo docker ps | grep mechasoft

# Iniciar se parado
sudo docker start mechasoft-sqlserver

# Aguardar inicialização
sleep 60
```

### ❌ "dotnet-ef não encontrado"

**Ambos:**
```bash
dotnet tool install --global dotnet-ef
```

### ❌ "Erro ao executar migrações"

**Ambos:**
```bash
# Verificar connection string
# Windows: MechaSoft.WebAPI\appsettings.Development.json
# Linux: MechaSoft.WebAPI/appsettings.Development.json

# Aguardar SQL Server iniciar
# Windows: Verificar serviço
# Linux: sleep 60

# Tentar novamente
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

---

## 📚 RECURSOS ADICIONAIS

### Documentação Oficial
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Angular Documentation](https://angular.io/docs)
- [SQL Server on Linux](https://docs.microsoft.com/sql/linux/)

### Ferramentas Úteis
- [Visual Studio Code](https://code.visualstudio.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Windows)
- [SQL Server Management Studio](https://docs.microsoft.com/sql/ssms/) (Windows)
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/) (Multiplataforma)
- [Postman](https://www.postman.com/) - Testar API

---

## 🎓 ESTRUTURA DO PROJETO

```
MechaSoftApp/
├── 📁 MechaSoft.Domain/           # Entidades e lógica de domínio
├── 📁 MechaSoft.Domain.Core/      # Interfaces e contratos
├── 📁 MechaSoft.Application/      # CQRS (Commands & Queries)
├── 📁 MechaSoft.Data/            # EF Core e repositórios
├── 📁 MechaSoft.Security/        # JWT e autenticação
├── 📁 MechaSoft.IoC/             # Injeção de dependências
├── 📁 MechaSoft.WebAPI/          # API REST (Endpoints)
├── 📁 Presentation/
│   └── 📁 MechaSoft.Angular/     # Frontend Angular
│
├── 📄 SETUP_WINDOWS.md           # 🪟 Guia Windows
├── 📄 SETUP_LINUX.md             # 🐧 Guia Linux
├── 📄 STATUS_PROJETO.md          # 📊 Estado do projeto
├── 📄 PROXIMOS_PASSOS.md         # 🎯 Próximos passos
├── 📄 GUIA_RAPIDO.md             # ⚡ Este ficheiro
│
├── 🔧 start-mechasoft.ps1        # 🪟 Iniciar (Windows)
├── 🔧 stop-mechasoft.ps1         # 🪟 Parar (Windows)
├── 🔧 setup-db.ps1               # 🪟 Setup DB (Windows)
│
├── 🔧 start-mechasoft.sh         # 🐧 Iniciar (Linux)
├── 🔧 stop-mechasoft.sh          # 🐧 Parar (Linux)
├── 🔧 setup-sqlserver.sh         # 🐧 Setup SQL Server (Linux)
└── 🔧 setup-db.sh                # 🐧 Setup DB (Linux)
```

---

## 📞 SUPORTE

Para problemas específicos:
1. Consulte a documentação da sua plataforma (SETUP_WINDOWS.md ou SETUP_LINUX.md)
2. Verifique STATUS_PROJETO.md para estado atual
3. Consulte PROXIMOS_PASSOS.md para comandos úteis

---

**🚀 Bom desenvolvimento!**

