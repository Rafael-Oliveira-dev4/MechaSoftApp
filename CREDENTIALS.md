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

### Base de Dados (SQL Server)
```
Server:   localhost,1433
Database: DV_RO_MechaSoft
User:     sa
Password: CHANGE_ME_PASSWORD
```

---

## 🚀 Como Iniciar o Projeto

### Opção 1: Script Automatizado (Linux)
```bash
./start-mechasoft.sh
```

### Opção 2: Manual

**1. Iniciar SQL Server (Docker):**
```bash
./setup-sqlserver.sh
```

**2. Iniciar Backend (.NET 8):**
```bash
cd MechaSoft.WebAPI
dotnet run
```

**3. Iniciar Frontend (Angular 20):**
```bash
cd Presentation/MechaSoft.Angular
npm start
```

---

## 📝 Notas Importantes

- ⚠️ **NUNCA** commitar este ficheiro com credenciais reais em produção
- 🔒 As passwords devem ser alteradas em ambiente de produção
- 🧪 Estas credenciais são apenas para desenvolvimento/teste local
- 📊 Os dados de seed estão em: `scripts/seed-data.sql` e `scripts/seed-services.sql`

---

**Última Atualização:** 13 de Outubro de 2025

