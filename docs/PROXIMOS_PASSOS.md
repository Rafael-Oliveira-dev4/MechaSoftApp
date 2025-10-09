# 🚀 MechaSoft - Próximos Passos

## 📊 ESTADO ATUAL

✅ **Backend:** A correr em http://localhost:5039  
✅ **Frontend:** A correr em http://localhost:4300  
✅ **Swagger:** Acessível em http://localhost:5039/swagger  
❌ **Base de Dados:** SQL Server NÃO configurado

---

## ⚠️ PROBLEMA CRÍTICO: Base de Dados

O backend está a correr mas **falhará** em qualquer operação que aceda à base de dados porque o SQL Server não está configurado.

### 🔧 SOLUÇÃO RÁPIDA: Configurar SQL Server

#### Opção 1: Docker (Recomendado)

```bash
# 1. Instalar Docker
sudo apt update
sudo apt install -y docker.io
sudo systemctl start docker
sudo systemctl enable docker
sudo usermod -aG docker $USER

# 2. Reiniciar sessão ou executar:
newgrp docker

# 3. Executar o script de setup
cd /home/rick/Documents/GitHub/MechaSoftApp
./setup-sqlserver.sh

# 4. Verificar se SQL Server está rodando
docker ps | grep mechasoft-sqlserver

# 5. Executar migrações
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

#### Opção 2: SQL Server Existente

Se já tiver um SQL Server disponível:

```bash
# 1. Atualizar connection string em:
# MechaSoft.WebAPI/appsettings.Development.json

# 2. Executar migrações
cd /home/rick/Documents/GitHub/MechaSoftApp
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

---

## 📋 CHECKLIST DE CONFIGURAÇÃO

### Fase 1: Base de Dados ❌
- [ ] Docker instalado e rodando
- [ ] SQL Server container criado e iniciado
- [ ] Migrações executadas com sucesso
- [ ] Tabelas criadas na base de dados

### Fase 2: Testes Funcionais ⏳
- [ ] Aceder ao Swagger: http://localhost:5039/swagger
- [ ] Testar endpoint de Health Check
- [ ] Testar criação de Cliente (Customer)
- [ ] Testar listagem de Clientes
- [ ] Verificar dados na base de dados

### Fase 3: Frontend ⏳
- [ ] Aceder ao Angular: http://localhost:4300
- [ ] Testar navegação entre páginas
- [ ] Testar formulários de cadastro
- [ ] Verificar comunicação com backend

---

## 🎯 SEQUÊNCIA DE AÇÕES RECOMENDADA

### 1️⃣ Configurar Base de Dados (AGORA)

```bash
# Executar como root/sudo
sudo apt update
sudo apt install -y docker.io
sudo systemctl start docker
sudo usermod -aG docker $USER
newgrp docker

# Executar script de setup
cd /home/rick/Documents/GitHub/MechaSoftApp
./setup-sqlserver.sh
```

### 2️⃣ Executar Migrações

```bash
cd /home/rick/Documents/GitHub/MechaSoftApp
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

### 3️⃣ Testar Backend via Swagger

1. Abrir: http://localhost:5039/swagger
2. Testar endpoint `POST /api/customers` para criar um cliente
3. Testar endpoint `GET /api/customers` para listar clientes

Exemplo de JSON para criar cliente:
```json
{
  "firstName": "João",
  "lastName": "Silva",
  "email": "joao.silva@example.com",
  "phone": "912345678",
  "nif": "123456789",
  "street": "Rua Principal",
  "number": "123",
  "parish": "Santa Maria",
  "municipality": "Lisboa",
  "district": "Lisboa",
  "postalCode": "1000-001"
}
```

### 4️⃣ Testar Frontend

1. Abrir: http://localhost:4300
2. Navegar pelas páginas
3. Testar formulários

---

## 🛠️ COMANDOS ÚTEIS

### Verificar Estado das Aplicações

```bash
# Ver processos rodando
ps aux | grep -E "(MechaSoft.WebAP|ng serve)" | grep -v grep

# Ver portas em uso
ss -tulpn | grep -E ':(5039|4300)'

# Testar health check
curl http://localhost:5039/health
```

### Gerir Aplicações

```bash
# PARAR aplicações
pkill -f "MechaSoft.WebAPI"
pkill -f "ng serve"

# INICIAR aplicações
./start-mechasoft.sh

# Ver logs do SQL Server
docker logs mechasoft-sqlserver

# Conectar ao SQL Server
docker exec -it mechasoft-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P "CHANGE_ME_PASSWORD"
```

### Gerir Base de Dados

```bash
# Ver migrations disponíveis
dotnet ef migrations list --project MechaSoft.Data --startup-project MechaSoft.WebAPI

# Criar nova migration
dotnet ef migrations add NomeDaMigracao --project MechaSoft.Data --startup-project MechaSoft.WebAPI

# Aplicar migrations
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI

# Reverter migration
dotnet ef database update NomeMigracaoAnterior --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

---

## 🔐 CREDENCIAIS SQL SERVER

### Informações de Acesso
- **Host:** localhost
- **Porta:** 1433
- **Database:** DV_RO_MechaSoft
- **Utilizador:** sa
- **Password:** CHANGE_ME_PASSWORD

### Connection String
```
Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true
```

---

## 📚 PRÓXIMAS FUNCIONALIDADES A IMPLEMENTAR

### 1. Autenticação e Autorização
- [ ] Endpoints de Login/Register já existem
- [ ] Implementar middleware de autenticação JWT
- [ ] Adicionar roles (Admin, Mechanic, Customer)
- [ ] Proteger endpoints com [Authorize]

### 2. Funcionalidades de Negócio
- [ ] Gestão completa de Clientes ✅
- [ ] Gestão de Veículos
- [ ] Gestão de Peças (Parts)
- [ ] Gestão de Serviços (Services)
- [ ] Ordens de Serviço (Service Orders)
- [ ] Inspeções (Inspections)
- [ ] Dashboard com estatísticas

### 3. Frontend Angular
- [ ] Páginas de listagem com paginação
- [ ] Formulários de cadastro/edição
- [ ] Validações client-side
- [ ] Interceptor HTTP para autenticação
- [ ] Guards para rotas protegidas
- [ ] Notificações de sucesso/erro

### 4. Testes
- [ ] Testes unitários (xUnit)
- [ ] Testes de integração
- [ ] Testes E2E (Playwright/Cypress)

### 5. DevOps
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Docker Compose para toda a stack
- [ ] Configurações de produção
- [ ] Logging centralizado (Serilog)
- [ ] Monitorização (Health Checks)

---

## ⚡ RESOLUÇÃO DE PROBLEMAS

### Backend não conecta à base de dados
```bash
# Verificar se SQL Server está rodando
docker ps | grep mechasoft-sqlserver

# Se não estiver, iniciar
docker start mechasoft-sqlserver

# Verificar logs
docker logs mechasoft-sqlserver
```

### Frontend não consegue chamar API
- Verificar se backend está rodando: `curl http://localhost:5039/health`
- Verificar configuração CORS em `appsettings.json`
- Verificar URL em `environment.development.ts`

### Erro ao executar migrações
```bash
# Limpar e recompilar
dotnet clean
dotnet restore
dotnet build

# Tentar executar migrações novamente
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

### Porta já em uso
```bash
# Identificar processo usando a porta
lsof -i :5039
lsof -i :4300

# Matar processo
kill -9 <PID>
```

---

## 📞 RECURSOS ADICIONAIS

### Documentação
- **README.md** - Visão geral do projeto
- **SETUP.md** - Instruções detalhadas de configuração
- **Swagger** - http://localhost:5039/swagger (documentação da API)

### Estrutura do Projeto
```
MechaSoftApp/
├── MechaSoft.Domain/          # Entidades de domínio
├── MechaSoft.Domain.Core/     # Interfaces e contratos
├── MechaSoft.Application/     # CQRS Handlers (MediatR)
├── MechaSoft.Data/           # Repositórios e EF Core
├── MechaSoft.Security/       # JWT e autenticação
├── MechaSoft.IoC/            # Dependency Injection
├── MechaSoft.WebAPI/         # API Endpoints
└── Presentation/
    └── MechaSoft.Angular/    # Frontend Angular
```

---

**Última Atualização:** 09/10/2025  
**Versão Backend:** .NET 8.0  
**Versão Frontend:** Angular 20.1.0

