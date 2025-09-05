# MechaSoft App - Setup Guide

## ✅ Dependências Instaladas

### .NET 8.0 SDK

- **Versão**: 8.0.407
- **Instalado via**: Snap
- **Comando**: `sudo snap install dotnet-sdk --classic`

### Node.js e npm

- **npm versão**: 10.9.3
- **Status**: ✅ Já instalado

## 📦 Pacotes Restaurados

### Backend (.NET)

- ✅ Todos os pacotes NuGet foram restaurados com sucesso
- ✅ Projeto compila sem erros
- ✅ Dependências principais:
  - Entity Framework Core 9.0.7
  - MediatR 13.0.0
  - FluentValidation 12.0.0
  - ASP.NET Core 8.0

### Frontend (Angular)

- ✅ Dependências npm instaladas
- ✅ Angular 20.1.0
- ✅ TypeScript 5.8.2
- ✅ RxJS 7.8.0

## 🔧 Configurações do VS Code

### Extensões Recomendadas

As seguintes extensões foram configuradas no arquivo `.vscode/extensions.json`:

#### .NET e C# Development

- `ms-dotnettools.csharp` - Suporte completo para C#
- `ms-dotnettools.csdevkit` - Kit de desenvolvimento C#
- `ms-dotnettools.vscode-dotnet-runtime` - Runtime .NET

#### Angular Development

- `angular.ng-template` - Suporte para templates Angular
- `johnpapa.angular2` - Snippets e ferramentas Angular
- `cyrilletuzi.angular-schematics` - Angular Schematics

#### File Explorer e Visualização

- `alefragnani.project-manager` - Gerenciamento de projetos
- `pkief.material-icon-theme` - Ícones para arquivos e pastas
- `zhuangtongfa.material-theme` - Tema Material

#### Database e Entity Framework

- `ms-mssql.mssql` - Suporte para SQL Server
- `ms-dotnettools.dotnet-interactive-vscode` - .NET Interactive

#### Git e Version Control

- `eamodio.gitlens` - Git supercharged
- `github.vscode-pull-request-github` - Pull requests do GitHub

#### Code Quality e Formatting

- `esbenp.prettier-vscode` - Formatador de código
- `ms-vscode.vscode-eslint` - Linter JavaScript/TypeScript

### Configurações do Workspace

- ✅ Formatação automática ao salvar
- ✅ Organização automática de imports
- ✅ File nesting configurado para .NET e Angular
- ✅ Exclusão de pastas desnecessárias do explorer
- ✅ Configurações de terminal otimizadas

### Tarefas Configuradas

- ✅ Build Solution
- ✅ Restore NuGet Packages
- ✅ Run WebAPI
- ✅ Run Angular Dev Server
- ✅ Add Migration
- ✅ Update Database
- ✅ Clean Solution

### Debug Configurado

- ✅ Launch WebAPI
- ✅ Launch Angular
- ✅ Launch Full Stack (ambos simultaneamente)

## 🚀 Como Executar o Projeto

### Backend (WebAPI)

```bash
# Navegar para o diretório raiz
cd /home/rick/Documentos/Github/MechaSoftApp

# Executar a WebAPI
dotnet run --project MechaSoft.WebAPI
```

### Frontend (Angular)

```bash
# Navegar para o diretório Angular
cd Presentation/MechaSoft.Angular

# Executar o servidor de desenvolvimento
npm start
```

### Usando VS Code

1. Abrir o projeto no VS Code
2. Pressionar `Ctrl+Shift+P`
3. Digitar "Tasks: Run Task"
4. Selecionar "Launch Full Stack" para executar ambos

## 📁 Estrutura do Projeto

```
MechaSoftApp/
├── MechaSoft.Domain/          # Entidades de domínio
├── MechaSoft.Domain.Core/     # Interfaces e contratos
├── MechaSoft.Application/     # Lógica de aplicação (CQRS, MediatR)
├── MechaSoft.Data/           # Acesso a dados (EF Core)
├── MechaSoft.IoC/            # Injeção de dependência
├── MechaSoft.WebAPI/         # API REST
└── Presentation/
    └── MechaSoft.Angular/    # Frontend Angular
```

## 🛠️ Comandos Úteis

### .NET

```bash
# Restaurar pacotes
dotnet restore

# Compilar solução
dotnet build

# Executar testes
dotnet test

# Adicionar migração
dotnet ef migrations add NomeDaMigracao --project MechaSoft.Data --startup-project MechaSoft.WebAPI

# Atualizar banco de dados
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

### Angular

```bash
# Instalar dependências
npm install

# Executar em modo desenvolvimento
npm start

# Compilar para produção
npm run build

# Executar testes
npm test
```

## 📝 Próximos Passos

1. **Configurar banco de dados**: Atualizar connection string no `appsettings.json`
2. **Executar migrações**: Criar e aplicar migrações do Entity Framework
3. **Configurar autenticação**: Implementar JWT Bearer authentication
4. **Desenvolver funcionalidades**: Começar a implementar os use cases
5. **Testes**: Adicionar testes unitários e de integração

## ⚠️ Observações

- O projeto compila com alguns warnings relacionados a nullable reference types, mas isso não impede o funcionamento
- As extensões do VS Code serão sugeridas automaticamente quando você abrir o projeto
- Certifique-se de ter o .NET 8.0 SDK e Node.js instalados antes de executar o projeto
