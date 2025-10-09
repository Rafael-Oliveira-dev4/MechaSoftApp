# 🐧 MechaSoft - Configuração Linux

**Plataforma:** Linux (Debian/Ubuntu)  
**Ambiente:** Bash

---

## 📋 PRÉ-REQUISITOS

### 1. .NET SDK 8.0
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install -y dotnet-sdk-8.0

# Ou via Snap
sudo snap install dotnet-sdk --classic

# Verificar instalação
dotnet --version
```

### 2. Node.js e npm
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install -y nodejs npm

# Ou via NodeSource (versão mais recente)
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt install -y nodejs

# Verificar instalação
node --version
npm --version
```

### 3. Docker (para SQL Server)
```bash
# Instalar Docker
sudo apt update
sudo apt install -y docker.io

# Iniciar e habilitar Docker
sudo systemctl start docker
sudo systemctl enable docker

# Adicionar utilizador ao grupo docker
sudo usermod -aG docker $USER

# Aplicar mudanças (logout/login ou executar)
newgrp docker

# Verificar
docker --version
```

---

## 🔧 CONFIGURAÇÃO DO SQL SERVER

### Via Docker (Recomendado para Linux)

```bash
# Executar script de setup
chmod +x setup-sqlserver.sh
sudo ./setup-sqlserver.sh

# OU manualmente:
docker run -e "ACCEPT_EULA=Y" \
           -e "SA_PASSWORD=CHANGE_ME_PASSWORD" \
           -e "MSSQL_PID=Express" \
           -p 1433:1433 \
           --name mechasoft-sqlserver \
           --hostname mechasoft-sqlserver \
           -d mcr.microsoft.com/mssql/server:2022-latest
```

### Connection String para Linux
Editar `MechaSoft.WebAPI/appsettings.Development.json`:

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
```bash
cd ~/Documents/GitHub
git clone [URL_DO_REPOSITORIO]
cd MechaSoftApp
```

### 2. Instalar Entity Framework Tools
```bash
dotnet tool install --global dotnet-ef
```

### 3. Restaurar Pacotes .NET
```bash
dotnet restore
```

### 4. Compilar Backend
```bash
dotnet build --configuration Release
```

### 5. Instalar Dependências Angular
```bash
cd Presentation/MechaSoft.Angular
npm install
cd ../..
```

### 6. Executar Migrações
```bash
# Usar o script automático
chmod +x setup-db.sh
./setup-db.sh

# OU manualmente:
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

---

## ▶️ EXECUTAR A APLICAÇÃO

### Opção 1: Script Bash

```bash
# Tornar scripts executáveis
chmod +x start-mechasoft.sh
chmod +x stop-mechasoft.sh

# Iniciar
./start-mechasoft.sh

# Parar
./stop-mechasoft.sh
```

### Opção 2: Comandos Manuais

**Backend:**
```bash
# Terminal 1
cd MechaSoft.WebAPI
dotnet run --urls "http://localhost:5039"
```

**Frontend:**
```bash
# Terminal 2
cd Presentation/MechaSoft.Angular
npm start -- --port 4300
```

---

## 🛠️ COMANDOS ÚTEIS (Linux)

### Gerir SQL Server (Docker)
```bash
# Ver status
docker ps | grep mechasoft-sqlserver

# Iniciar
docker start mechasoft-sqlserver

# Parar
docker stop mechasoft-sqlserver

# Reiniciar
docker restart mechasoft-sqlserver

# Ver logs
docker logs mechasoft-sqlserver

# Ver logs em tempo real
docker logs -f mechasoft-sqlserver

# Conectar via sqlcmd
docker exec -it mechasoft-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P "CHANGE_ME_PASSWORD" -C
```

### Gerir Aplicação
```bash
# Ver processos
ps aux | grep -E "(MechaSoft.WebAP|ng serve)" | grep -v grep

# Ver portas
ss -tulpn | grep -E ':(5039|4300)'
# OU
netstat -tulpn | grep -E ':(5039|4300)'

# Matar processos
pkill -f "MechaSoft.WebAPI"
pkill -f "ng serve"
```

### Testar Conectividade
```bash
# Health check
curl http://localhost:5039/health

# Swagger
curl -I http://localhost:5039/swagger/index.html

# Listar clientes
curl http://localhost:5039/api/customers?pageNumber=1&pageSize=10
```

---

## 🔍 TROUBLESHOOTING (Linux)

### Porta em uso
```bash
# Ver o que está usando a porta
lsof -i :5039
lsof -i :4300

# Matar processo por PID
kill -9 [PID]
```

### Docker: Permissão negada
```bash
# Adicionar utilizador ao grupo docker
sudo usermod -aG docker $USER

# Logout e login novamente
# OU executar
newgrp docker

# OU usar sudo
sudo docker ps
```

### SQL Server não inicia
```bash
# Ver logs
sudo docker logs mechasoft-sqlserver

# Remover e recriar container
sudo docker stop mechasoft-sqlserver
sudo docker rm mechasoft-sqlserver
sudo ./setup-sqlserver.sh
```

### Erro de conexão à base de dados
```bash
# Verificar se SQL Server está rodando
sudo docker ps | grep mechasoft

# Testar conectividade
nc -zv localhost 1433

# Aguardar inicialização
sleep 60
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

### Limpar e reinstalar
```bash
# Parar tudo
./stop-mechasoft.sh

# Limpar
dotnet clean
rm -rf Presentation/MechaSoft.Angular/node_modules

# Reinstalar
dotnet restore
cd Presentation/MechaSoft.Angular
npm install
cd ../..

# Configurar DB
./setup-db.sh

# Iniciar
./start-mechasoft.sh
```

---

## 🐳 DOCKER COMPOSE (Opcional)

### docker-compose.yml
```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: mechasoft-sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=CHANGE_ME_PASSWORD
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql

volumes:
  sqlserver_data:
```

### Usar Docker Compose
```bash
# Iniciar
docker-compose up -d

# Parar
docker-compose down

# Ver logs
docker-compose logs -f sqlserver
```

---

## 📦 BUILD PARA PRODUÇÃO

### Backend
```bash
dotnet publish -c Release -o ./publish
```

### Frontend
```bash
cd Presentation/MechaSoft.Angular
npm run build
# Output em: dist/MechaSoft.Angular/
```

---

## 🔐 CREDENCIAIS (Desenvolvimento)

### SQL Server (Docker)
- **Host:** localhost:1433
- **Database:** DV_RO_MechaSoft
- **User:** sa
- **Password:** CHANGE_ME_PASSWORD

### Connection String
```
Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true
```

---

## 📝 SYSTEMD SERVICE (Opcional)

### Criar serviço para o backend

```bash
sudo nano /etc/systemd/system/mechasoft-api.service
```

```ini
[Unit]
Description=MechaSoft WebAPI
After=network.target

[Service]
Type=simple
User=rick
WorkingDirectory=/home/rick/Documents/GitHub/MechaSoftApp/MechaSoft.WebAPI
ExecStart=/usr/bin/dotnet run --urls "http://localhost:5039"
Restart=on-failure

[Install]
WantedBy=multi-user.target
```

### Usar o serviço
```bash
# Recarregar systemd
sudo systemctl daemon-reload

# Habilitar
sudo systemctl enable mechasoft-api

# Iniciar
sudo systemctl start mechasoft-api

# Ver status
sudo systemctl status mechasoft-api

# Ver logs
journalctl -u mechasoft-api -f
```

---

**Preparado para Linux (Debian/Ubuntu)**  
**Bash Shell**

