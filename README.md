# MechaSoftApp

Solução full‑stack para gestão de oficina, com backend em .NET 8 (WebAPI + EF Core) e frontend em Angular.

## 🚀 Início Rápido

**Escolha o guia de configuração para sua plataforma:**

### 🪟 Windows
→ **[docs/SETUP_WINDOWS.md](docs/SETUP_WINDOWS.md)** - Guia completo para Windows 10/11  
→ **[scripts/windows/](scripts/windows/)** - Scripts PowerShell

### 🐧 Linux
→ **[docs/SETUP_LINUX.md](docs/SETUP_LINUX.md)** - Guia completo para Linux (Debian/Ubuntu)  
→ **[scripts/linux/](scripts/linux/)** - Scripts Bash

### 📚 Documentação Completa
→ **[docs/](docs/)** - Toda a documentação organizada  
→ **[docs/GUIA_RAPIDO.md](docs/GUIA_RAPIDO.md)** - ⚡ Referência rápida  
→ **[docs/STATUS_PROJETO.md](docs/STATUS_PROJETO.md)** - 📊 Estado atual  
→ **[docs/PROXIMOS_PASSOS.md](docs/PROXIMOS_PASSOS.md)** - 🎯 Próximos passos

---

## Pré‑requisitos Gerais

- .NET SDK 8.x
- Node.js 18+ e npm 9+
- Git
- SQL Server (SQL Express no Windows, Docker no Linux)

## 🏗️ Arquitetura

### Camadas do Projeto
- **MechaSoft.Domain** — Entidades de domínio e Value Objects
- **MechaSoft.Domain.Core** — Interfaces e contratos
- **MechaSoft.Application** — Lógica de aplicação (CQRS com MediatR)
- **MechaSoft.Data** — Acesso a dados (EF Core e repositórios)
- **MechaSoft.Security** — Autenticação JWT
- **MechaSoft.IoC** — Injeção de dependências
- **MechaSoft.WebAPI** — API REST (Minimal Endpoints)
- **Presentation/MechaSoft.Angular** — Frontend Angular 20

### Padrões Implementados
✅ Clean Architecture  
✅ CQRS (Command Query Responsibility Segregation)  
✅ Repository Pattern  
✅ Unit of Work  
✅ Domain-Driven Design  
✅ Value Objects (Money, Name, Address)

---

## ⚡ Guias de Configuração

### 📖 Documentação Completa

| Ficheiro | Descrição |
|----------|-----------|
| **[GUIA_RAPIDO.md](GUIA_RAPIDO.md)** | ⚡ Referência rápida - comandos essenciais |
| **[SETUP_WINDOWS.md](SETUP_WINDOWS.md)** | 🪟 Configuração detalhada para Windows |
| **[SETUP_LINUX.md](SETUP_LINUX.md)** | 🐧 Configuração detalhada para Linux |
| **[STATUS_PROJETO.md](STATUS_PROJETO.md)** | 📊 Estado atual e endpoints disponíveis |
| **[PROXIMOS_PASSOS.md](PROXIMOS_PASSOS.md)** | 🎯 Próximas funcionalidades e comandos úteis |

---

## 🚀 Início Rápido

### Windows
```powershell
# 1. Configurar base de dados
.\scripts\windows\setup-db.ps1

# 2. Iniciar aplicação
.\scripts\windows\start.ps1

# 3. Aceder
# - Backend:  http://localhost:5039
# - Swagger:  http://localhost:5039/swagger
# - Frontend: http://localhost:4300

# 4. Parar aplicação
.\scripts\windows\stop.ps1
```

### Linux
```bash
# 1. Configurar SQL Server (primeira vez)
sudo ./scripts/linux/setup-sqlserver.sh

# 2. Configurar base de dados
./scripts/linux/setup-db.sh

# 3. Iniciar aplicação
./scripts/linux/start.sh

# 4. Aceder
# - Backend:  http://localhost:5039
# - Swagger:  http://localhost:5039/swagger
# - Frontend: http://localhost:4300

# 5. Parar aplicação
./scripts/linux/stop.sh
```

---

## 📁 Estrutura do Projeto

```
MechaSoftApp/
├── docs/                         # 📚 Documentação
│   ├── GUIA_RAPIDO.md
│   ├── SETUP_WINDOWS.md
│   ├── SETUP_LINUX.md
│   ├── STATUS_PROJETO.md
│   └── PROXIMOS_PASSOS.md
│
├── scripts/                      # 🛠️ Scripts
│   ├── windows/                 # 🪟 PowerShell
│   └── linux/                   # 🐧 Bash
│
├── MechaSoft.Domain/             # 📦 Domínio
├── MechaSoft.Domain.Core/        # 🔌 Interfaces
├── MechaSoft.Application/        # 💼 CQRS Handlers
├── MechaSoft.Data/              # 🗄️ EF Core
├── MechaSoft.Security/          # 🔐 JWT Auth
├── MechaSoft.IoC/               # 🔧 DI
├── MechaSoft.WebAPI/            # 🌐 API REST
└── Presentation/
    └── MechaSoft.Angular/       # 🅰️ Frontend
```

---

## 🛠️ Scripts Disponíveis

### Windows (PowerShell)
Localização: `scripts/windows/`
- `start.ps1` — Inicia backend e frontend
- `stop.ps1` — Para aplicação
- `setup-db.ps1` — Configura base de dados

### Linux (Bash)
Localização: `scripts/linux/`
- `start.sh` — Inicia backend e frontend
- `stop.sh` — Para aplicação
- `setup-sqlserver.sh` — Configura SQL Server (Docker)
- `setup-db.sh` — Executa migrações

---

## 🔧 Tecnologias

### Backend
- .NET 8.0
- Entity Framework Core 9.0
- MediatR 13.0
- FluentValidation 12.0
- JWT Bearer Authentication
- Swagger/OpenAPI

### Frontend
- Angular 20.1
- TypeScript 5.8
- RxJS 7.8
- Tailwind CSS

### Base de Dados
- SQL Server 2022
- Entity Framework Core Migrations

---

## 📝 Convenções

- ✅ Mensagens de commit em inglês (ex.: `feat(api): add customer endpoints`)
- ✅ Endpoints minimalistas (sem controllers)
- ✅ CQRS para separação de responsabilidades
- ✅ Configurações em ficheiros (não automáticas)
- ✅ Documentação sempre atualizada

---

## 📞 Suporte

Para dúvidas:
1. Consulte [docs/GUIA_RAPIDO.md](docs/GUIA_RAPIDO.md)
2. Verifique [docs/STATUS_PROJETO.md](docs/STATUS_PROJETO.md)
3. Veja documentação específica:
   - Windows: [docs/SETUP_WINDOWS.md](docs/SETUP_WINDOWS.md)
   - Linux: [docs/SETUP_LINUX.md](docs/SETUP_LINUX.md)

---

**Desenvolvido com ❤️ para gestão de oficinas**