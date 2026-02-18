# 🔐 MechaSoft - Credenciais de Acesso

## 👤 Utilizadores Disponíveis

### 1️⃣ **Administrador / Owner**
```
Username: admin
Email:    admin@mechasoft.pt
Password: Admin123!
Role:     Owner (Acesso Total)
```

**Permissões:**
- ✅ Dashboard Administrativo completo
- ✅ Gestão de Clientes, Veículos, Ordens
- ✅ Gestão de Peças e Stock
- ✅ Gestão de Serviços de Oficina
- ✅ Visualização de Inspeções
- ✅ Relatórios e Estatísticas

---

### 2️⃣ **Cliente**
```
Username: maria_santos
Email:    maria.santos@email.pt
Password: Customer123!
Role:     Customer (Acesso Limitado)
```

> **Nota:** Este utilizador está ligado ao cliente "Maria Santos" na base de dados, com 1 veículo (Renault Clio) e ordens de serviço associadas.

**Permissões:**
- ✅ Dashboard do Cliente (vista limitada)
- ✅ Visualizar seus Veículos
- ✅ Visualizar suas Ordens de Serviço
- ✅ Perfil e Configurações pessoais
- ❌ Sem acesso a gestão administrativa

---

### 3️⃣ **Funcionário (Auto-Created via Backend)**
```
Username: ana.silva
Email:    ana.silva@mechasoft.pt
Password: Temp@8113
Role:     Employee (Mecânico)
```

> **Nota:** Esta conta foi criada **automaticamente** quando o funcionário "Ana Silva" foi adicionado ao sistema. A feature **Auto-Create User** cria um utilizador linkado ao Employee com credenciais temporárias.

**Permissões:**
- ✅ Dashboard do Funcionário
- ✅ Gestão de Ordens de Serviço atribuídas
- ✅ Visualizar Peças e Serviços
- ⚠️ **DEVE alterar a senha no primeiro login**

---

## 🌐 URLs de Acesso

### Frontend (Angular)
```
URL:     http://localhost:4200
Login:   http://localhost:4200/login
```

### Backend (WebAPI)
```
API:     http://localhost:5039
Swagger: http://localhost:5039/swagger
```

### Base de Dados (SQL Server Local - LEGACY)
```
Server:   localhost,1433
Database: DV_RO_MechaSoft
User:     sa
Password: CHANGE_ME_PASSWORD
```
> ⚠️ **DESCONTINUADO:** Apenas para desenvolvimento local sem Azure.

### Base de Dados (Azure SQL - PRODUÇÃO/CLOUD)
```
Server:   mechasoft-server-2025.database.windows.net,1433
Database: DV_RO_MechaSoft
User:     mechasoft_admin
Password: Azure2025@Secure!

Connection String:
Server=tcp:mechasoft-server-2025.database.windows.net,1433;
Initial Catalog=DV_RO_MechaSoft;
Persist Security Info=False;
User ID=mechasoft_admin;
Password=Azure2025@Secure!;
MultipleActiveResultSets=True;
Encrypt=True;
TrustServerCertificate=False;
Connection Timeout=30;
```

**Características:**
- ✅ **Região:** West Europe (próximo de Portugal)
- ✅ **Limites Gratuitos:** 100.000 vCore seconds + 32GB dados/mês
- ✅ **Acessível:** De qualquer computador com internet
- ✅ **Backup:** Automático pelo Azure

**Portal Azure:**
- 🌐 https://portal.azure.com
- 📧 Login: rafaeloliveirarafa04@gmail.com
- 📁 Resource Group: `mechasoft-rg`
- 💾 Server: `mechasoft-server-2025`

---

## 🚀 Como Iniciar o Projeto

### Opção 1: Com Azure SQL (Recomendado)
**Já está configurado! Basta iniciar:**

**1. Iniciar Backend (.NET 8):**
```bash
cd MechaSoft.WebAPI
dotnet run
```

**2. Iniciar Frontend (Angular 19):**
```bash
cd Presentation/MechaSoft.Angular
npm start
```

**3. (Opcional) Alimentar Base de Dados:**
```powershell
.\SeedCompleteDatabase.ps1
```

### Opção 2: Com SQL Server Local (Docker - Linux)
```bash
./start-mechasoft.sh
```

---

## 📊 Dados de Seed

Execute o script para popular a base de dados:
```powershell
.\SeedCompleteDatabase.ps1
```

**Dados criados:**
- 2 Utilizadores (Admin + Cliente)
- 3 Clientes
- 3 Funcionários
- 17 Serviços de Oficina
- 47 Peças em Stock

---

## 📝 Notas Importantes

- ⚠️ **NUNCA** commitar este ficheiro com credenciais reais em produção
- 🔒 As passwords devem ser alteradas em ambiente de produção
- 🧪 Estas credenciais são para desenvolvimento/teste
- 🌐 **Azure SQL Database** é a configuração ATIVA (já configurada nos appsettings)
- 💾 Connection strings já atualizadas em `appsettings.json` e `appsettings.Development.json`

---

**Última Atualização:** 14 de Outubro de 2025

