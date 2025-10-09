# 🐧 Scripts Linux (Bash)

Scripts para gerir a aplicação MechaSoft no Linux (Debian/Ubuntu).

---

## 📜 Scripts Disponíveis

### `start.sh` - Iniciar Aplicação
Inicia o backend (.NET) e frontend (Angular).

```bash
./start.sh
```

**O que faz:**
- ✅ Verifica se SQL Server Docker está rodando
- ▶️ Inicia Backend WebAPI na porta 5039
- ▶️ Inicia Frontend Angular na porta 4300
- 📊 Mostra URLs de acesso

---

### `stop.sh` - Parar Aplicação
Para todos os processos do backend e frontend.

```bash
./stop.sh
```

**O que faz:**
- 🔍 Procura processos MechaSoft.WebAPI
- 🔍 Procura processos Node.js (ng serve)
- ⏹️ Para todos os processos encontrados

---

### `setup-sqlserver.sh` - Configurar SQL Server
Cria e inicia container Docker com SQL Server 2022.

```bash
sudo ./setup-sqlserver.sh
```

**O que faz:**
- 🔍 Verifica se Docker está instalado
- 🔍 Verifica se .NET SDK está instalado
- 🛑 Para container existente (se houver)
- 🐳 Cria container SQL Server 2022
- ⏳ Aguarda inicialização (30 segundos)
- 🔧 Atualiza connection string em appsettings.Development.json

---

### `setup-db.sh` - Configurar Base de Dados
Executa migrações do Entity Framework.

```bash
./setup-db.sh
```

**O que faz:**
- 🔍 Verifica se container SQL Server existe e está rodando
- ⏳ Aguarda SQL Server estar pronto (60 segundos)
- 🚀 Executa migrações do Entity Framework
- ✅ Confirma sucesso

---

## 🚀 Sequência de Uso (Primeira Vez)

```bash
# 1. Navegar para a pasta de scripts
cd scripts/linux

# 2. Tornar scripts executáveis
chmod +x *.sh

# 3. Configurar SQL Server (primeira vez, requer sudo)
sudo ./setup-sqlserver.sh

# 4. Configurar base de dados
./setup-db.sh

# 5. Voltar à raiz e iniciar aplicação
cd ../..
./scripts/linux/start.sh

# 6. Aceder aplicação
# - Backend:  http://localhost:5039
# - Swagger:  http://localhost:5039/swagger
# - Frontend: http://localhost:4300

# 7. Parar quando terminar
./scripts/linux/stop.sh
```

---

## 📋 Pré-requisitos

- ✅ .NET SDK 8.0+
- ✅ Node.js 18+ e npm
- ✅ Docker
- ✅ Bash shell

### Instalar Pré-requisitos (Ubuntu/Debian)

```bash
# .NET SDK
sudo snap install dotnet-sdk --classic

# Node.js
sudo apt update
sudo apt install -y nodejs npm

# Docker
sudo apt update
sudo apt install -y docker.io
sudo systemctl start docker
sudo systemctl enable docker

# Adicionar utilizador ao grupo docker
sudo usermod -aG docker $USER
newgrp docker  # ou logout/login
```

---

## 🔧 Configuração SQL Server

### Docker (Padrão para Linux)

O script `setup-sqlserver.sh` cria automaticamente o container.

Connection string em `MechaSoft.WebAPI/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Credenciais:**
- User: `sa`
- Password: `CHANGE_ME_PASSWORD`
- Port: `1433`

---

## 🐳 Gerir SQL Server Docker

```bash
# Ver status
sudo docker ps | grep mechasoft-sqlserver

# Iniciar
sudo docker start mechasoft-sqlserver

# Parar
sudo docker stop mechasoft-sqlserver

# Reiniciar
sudo docker restart mechasoft-sqlserver

# Ver logs
sudo docker logs mechasoft-sqlserver

# Remover container
sudo docker stop mechasoft-sqlserver
sudo docker rm mechasoft-sqlserver
```

---

## 🆘 Resolução de Problemas

### Docker: Permission denied
```bash
# Adicionar ao grupo docker
sudo usermod -aG docker $USER

# Logout e login novamente, ou
newgrp docker
```

### Porta em uso
```bash
# Ver o que está usando a porta
lsof -i :5039
lsof -i :4300

# Matar processo
kill -9 [PID]
```

### SQL Server não inicia
```bash
# Ver logs
sudo docker logs mechasoft-sqlserver

# Remover e recriar
sudo docker stop mechasoft-sqlserver
sudo docker rm mechasoft-sqlserver
sudo ./setup-sqlserver.sh
```

### Script sem permissão de execução
```bash
chmod +x *.sh
```

---

📖 **Ver documentação completa:** [docs/SETUP_LINUX.md](../../docs/SETUP_LINUX.md)

