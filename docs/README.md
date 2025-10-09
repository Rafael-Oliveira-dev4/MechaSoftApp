# 📚 Documentação MechaSoft

Documentação completa do projeto MechaSoft organizada por plataforma e finalidade.

---

## 📖 Guias por Plataforma

### 🪟 Windows

**[SETUP_WINDOWS.md](SETUP_WINDOWS.md)** - Configuração Completa para Windows
- Pré-requisitos para Windows 10/11
- Instalação do SQL Server Express
- Scripts PowerShell
- Configuração do Visual Studio
- Troubleshooting específico do Windows

**Scripts Windows:** [`../scripts/windows/`](../scripts/windows/)

---

### 🐧 Linux

**[SETUP_LINUX.md](SETUP_LINUX.md)** - Configuração Completa para Linux
- Pré-requisitos para Debian/Ubuntu
- Instalação via Docker
- Scripts Bash
- Systemd services
- Troubleshooting específico do Linux

**Scripts Linux:** [`../scripts/linux/`](../scripts/linux/)

---

## 📚 Documentação Geral

### ⚡ [GUIA_RAPIDO.md](GUIA_RAPIDO.md)
**Referência Rápida** - Comandos essenciais
- Tabela de comparação Windows vs Linux
- Comandos mais usados
- Connection strings por plataforma
- Resolução rápida de problemas comuns

---

### 📊 [STATUS_PROJETO.md](STATUS_PROJETO.md)
**Estado Atual do Projeto**
- Sistema completo funcionando
- Endpoints disponíveis
- Exemplos de uso da API
- Estrutura do projeto
- Próximos desenvolvimentos

---

### 🎯 [PROXIMOS_PASSOS.md](PROXIMOS_PASSOS.md)
**Próximas Funcionalidades**
- Comandos úteis
- Credenciais de desenvolvimento
- Gestão do SQL Server
- Entity Framework migrations
- Troubleshooting detalhado

---

### 🔧 [SETUP.md](SETUP.md)
**Configuração Original** (Mantido para referência)
- Documentação original do projeto
- Configurações técnicas detalhadas

---

## 🗺️ Mapa de Navegação

```
📚 Sou novo no projeto?
   └─→ Leia: GUIA_RAPIDO.md
       ├─→ Windows? → SETUP_WINDOWS.md
       └─→ Linux?   → SETUP_LINUX.md

📊 Quer ver o estado atual?
   └─→ Leia: STATUS_PROJETO.md

🎯 Quer saber o que falta fazer?
   └─→ Leia: PROXIMOS_PASSOS.md

🔧 Precisa de comandos específicos?
   └─→ Leia: GUIA_RAPIDO.md → Seção específica

🆘 Tem problemas?
   ├─→ Windows? → SETUP_WINDOWS.md → Troubleshooting
   └─→ Linux?   → SETUP_LINUX.md → Troubleshooting
```

---

## 📁 Estrutura de Ficheiros

```
docs/
├── README.md                  # 📍 Você está aqui
├── GUIA_RAPIDO.md            # ⚡ Referência rápida
├── SETUP_WINDOWS.md          # 🪟 Guia Windows
├── SETUP_LINUX.md            # 🐧 Guia Linux
├── STATUS_PROJETO.md         # 📊 Estado atual
├── PROXIMOS_PASSOS.md        # 🎯 Próximos passos
└── SETUP.md                  # 🔧 Setup original
```

---

## 🔗 Links Rápidos

### Documentação Externa
- [.NET 8 Documentation](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Angular 20 Documentation](https://angular.io/docs)
- [MediatR Documentation](https://github.com/jbogard/MediatR/wiki)
- [FluentValidation](https://docs.fluentvalidation.net/)

### Ferramentas Recomendadas
- [Visual Studio Code](https://code.visualstudio.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Windows)
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/)
- [Postman](https://www.postman.com/) - Testar API
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

## 🎓 Aprenda Mais

### Arquitetura do Projeto
- **Clean Architecture** - Separação em camadas
- **CQRS** - Command Query Responsibility Segregation
- **Repository Pattern** - Abstração de dados
- **Domain-Driven Design** - Lógica de negócio no domínio
- **Value Objects** - Money, Name, Address

### Tecnologias Utilizadas
- **.NET 8** - Framework backend
- **EF Core 9** - ORM
- **MediatR** - CQRS implementation
- **FluentValidation** - Validações
- **JWT** - Autenticação
- **Angular 20** - Framework frontend
- **Tailwind CSS** - Styling
- **SQL Server 2022** - Base de dados

---

## 📞 Suporte

**Primeiro passo:** Consulte a documentação da sua plataforma
- Windows: [SETUP_WINDOWS.md](SETUP_WINDOWS.md)
- Linux: [SETUP_LINUX.md](SETUP_LINUX.md)

**Problemas comuns:** [GUIA_RAPIDO.md](GUIA_RAPIDO.md) → Seção "Resolução de Problemas"

---

**Última Atualização:** 09/10/2025  
**Versão Backend:** .NET 8.0  
**Versão Frontend:** Angular 20.1.0

