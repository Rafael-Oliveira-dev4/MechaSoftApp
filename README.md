# MechaSoft - Workshop Management System

Full-stack workshop management platform with a .NET backend and an Angular frontend.

## Stack

- Backend: .NET 10, ASP.NET Core Minimal API, EF Core, MediatR, FluentValidation
- Frontend: Angular 20, TypeScript, Tailwind CSS
- Database: SQL Server

## Repository Structure

```text
MechaSoftApp/
|- MechaSoft.WebAPI/          # ASP.NET Core API
|- MechaSoft.Application/     # Use cases and handlers
|- MechaSoft.Data/            # EF Core context and repositories
|- MechaSoft.Domain/          # Domain entities
|- MechaSoft.Domain.Core/     # Shared abstractions
|- MechaSoft.Security/        # Authentication and authorization
|- MechaSoft.IoC/             # Dependency injection
|- Presentation/
|  \- MechaSoft.Angular/      # Angular application
|- scripts/
|  |- linux/
|  \- windows/
\- API_TESTS.http
```

## Prerequisites

- .NET SDK 10+
- Node.js 18+ and npm 9+
- SQL Server (local or Docker)

## Configuration

1. Copy example files in `MechaSoft.WebAPI`:
   - `appsettings.example.json` -> `appsettings.json`
   - `appsettings.Development.example.json` -> `appsettings.Development.json`
2. Fill local values for:
   - `ConnectionStrings:MechaSoftCS`
   - `JwtSettings:Key`

Do not commit real credentials or secret keys.

## Run Locally

### Windows

```powershell
# from repository root
.\scripts\windows\setup-db.ps1
.\scripts\windows\start.ps1
```

### Linux/macOS

```bash
# from repository root
./scripts/linux/setup-db.sh
./scripts/linux/start.sh
```

### Manual run

Backend:

```bash
dotnet restore
dotnet run --project MechaSoft.WebAPI
```

Frontend:

```bash
cd Presentation/MechaSoft.Angular
npm install
npm start
```

Default local endpoints:

- Frontend: `http://localhost:4300`
- Backend: `http://localhost:5039`
- Swagger: `http://localhost:5039/swagger`

## Tests and Quality

Backend:

```bash
dotnet test
```

Frontend:

```bash
cd Presentation/MechaSoft.Angular
npm run build
npm run test -- --watch=false --browsers=ChromeHeadless
```

API manual tests:

- Open `API_TESTS.http` with a REST client extension.

## Documentation

- Angular structure: `Presentation/MechaSoft.Angular/ESTRUTURA.md`
- Navigation flow: `Presentation/MechaSoft.Angular/FLUXO_NAVEGACAO.md`

## Governance

- Contribution guide: `CONTRIBUTING.md`
- Code of conduct: `CODE_OF_CONDUCT.md`
- Security policy: `SECURITY.md`
- License: `LICENSE`

## Developer

- Rafael Oliveira

# 🔧 MechaSoft - Workshop Management System

Full-stack workshop management solution with .NET 10 backend (WebAPI + EF Core) and Angular 19 frontend.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-19-DD0031?logo=angular)](https://angular.io/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?logo=typescript)](https://www.typescriptlang.org/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)

---

## 📋 Features

### ✨ **User Management**
- 🔐 JWT authentication with refresh tokens
- 👤 User registration with async validation
- 📸 Profile image upload
- ⚙️ User settings and preferences
- 🔑 Password reset flow

### 🚗 **Workshop Management**
- 👥 Customer management with history
- 🚙 Vehicle tracking and maintenance history
- 📝 Service orders with status tracking
- 🔍 Technical inspections scheduling
- 🛠️ Service catalog management
- 📦 Parts inventory with stock alerts

### 📊 **Dashboard & Analytics**
- 📈 Real-time statistics
- 💰 Revenue tracking (daily, monthly, yearly)
- 📉 Low stock alerts
- 📋 Recent orders timeline

### 🎨 **Modern UI/UX**
- 🌙 Dark theme with gradients
- ✨ Smooth animations and transitions
- 📱 Fully responsive design
- ♿ Accessibility features
- 🎭 Glassmorphism effects

---

## 🏗️ Architecture

### Backend (.NET 10)
```
MechaSoft.WebAPI          # Minimal API endpoints
├── MechaSoft.Application # CQRS (MediatR)
├── MechaSoft.Domain      # Domain models
├── MechaSoft.Data        # EF Core + Repositories
├── MechaSoft.Security    # JWT & Authentication
└── MechaSoft.IoC         # Dependency Injection
```

### Frontend (Angular 19)
```
Presentation/MechaSoft.Angular
├── core/                 # Guards, Interceptors, Services
├── shared/               # Reusable Components
└── components/           # Feature Modules
    ├── auth/            # Login, Register
    ├── landing/         # Public landing page
    ├── front-office/    # Management system
    └── back-office/     # Admin panel (future)
```

---

## 🚀 Quick Start

### Prerequisites

- **.NET SDK 10.0+**
- **Node.js 18+ & npm 9+**
- **Docker** (for SQL Server on Linux)
- **Git**

### Setup & Run

#### **Linux / macOS**

```bash
# 1. Clone repository
git clone https://github.com/Rick-Sanchez4/MechaSoftApp.git
cd MechaSoftApp

# 2. Setup SQL Server (Docker)
./setup-sqlserver.sh

# 3. Start all services
./start-mechasoft.sh

# Access:
# - Frontend: http://localhost:4200
# - Backend:  http://localhost:5039
```

#### **Windows**

```powershell
# 1. Clone repository
git clone https://github.com/Rick-Sanchez4/MechaSoftApp.git
cd MechaSoftApp

# 2. Setup database (requires SQL Server LocalDB or Express)
# Update connection string in MechaSoft.WebAPI/appsettings.Development.json

# 3. Run migrations
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI

# 4. Start backend
cd MechaSoft.WebAPI
dotnet run

# 5. Start frontend (new terminal)
cd Presentation\MechaSoft.Angular
npm install
npm start

# Access:
# - Frontend: http://localhost:4200
# - Backend:  http://localhost:5039
```

---

## 🗂️ Project Structure

```
MechaSoftApp/
├── MechaSoft.WebAPI/         # ASP.NET Core API
├── MechaSoft.Application/    # Business logic (CQRS)
├── MechaSoft.Data/           # EF Core + Migrations
├── MechaSoft.Domain/         # Domain models
├── MechaSoft.Domain.Core/    # Shared interfaces
├── MechaSoft.Security/       # JWT services
├── MechaSoft.IoC/            # DI configuration
├── Presentation/
│   └── MechaSoft.Angular/    # Angular 19 frontend
├── setup-sqlserver.sh        # SQL Server setup (Linux)
├── start-mechasoft.sh        # Start all services (Linux)
├── stop-mechasoft.sh         # Stop all services (Linux)
└── API_TESTS.http            # API test collection
```

---

## 🔑 API Endpoints

### **Authentication**
```http
POST   /api/accounts/register               # Register new user
POST   /api/accounts/login                  # Login
POST   /api/accounts/refresh-token          # Refresh JWT token
POST   /api/accounts/logout                 # Logout
POST   /api/accounts/change-password        # Change password
POST   /api/accounts/reset-password         # Reset password
```

### **Profile**
```http
GET    /api/accounts/profile                # Get user profile
POST   /api/accounts/check-email            # Check email availability
POST   /api/accounts/check-username         # Check username availability
GET    /api/accounts/suggest-username       # Get username suggestions
POST   /api/accounts/upload-profile-image   # Upload profile image
```

### **Customers**
```http
GET    /api/customers                       # List all customers
GET    /api/customers/{id}                  # Get customer by ID
POST   /api/customers                       # Create customer
PUT    /api/customers/{id}                  # Update customer
DELETE /api/customers/{id}                  # Delete customer
```

### **Vehicles, Service Orders, Parts, Services, Inspections**
Similar CRUD endpoints for each module.

---

## 🗺️ Routes

### **Public Routes**
```
/                    → Landing page
/login               → User login
/register            → User registration
```

### **Authenticated Routes** (`/admin`)
```
/admin/dashboard       → Dashboard with statistics
/admin/profile         → User profile
/admin/settings        → User settings
/admin/customers       → Customer management
/admin/vehicles        → Vehicle management
/admin/service-orders  → Service order management
/admin/inspections     → Inspection scheduling
/admin/services        → Service catalog
/admin/parts           → Parts inventory
```

---

## 🔐 User Roles

| Role | Access Level |
|------|-------------|
| **Owner** | Full access to all modules |
| **Admin** | Management features + reports |
| **Employee** | Daily operations (orders, inspections) |
| **Customer** | Portal access (future feature) |

---

## 💾 Database

### **Technology**
- SQL Server 2022 (Docker on Linux)
- Entity Framework Core 8
- Code-First migrations

### **Migrations**
```bash
# View migrations
dotnet ef migrations list --project MechaSoft.Data --startup-project MechaSoft.WebAPI

# Apply migrations
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI

# Create new migration
dotnet ef migrations add MigrationName --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

### **Connection String (Linux/Docker)**
```json
{
  "ConnectionStrings": {
    "MechaSoftCS": "Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 🧪 Testing

### **API Tests**
Use `API_TESTS.http` file with REST Client extension for VS Code:

```http
### Register User
POST http://localhost:5039/api/accounts/register
Content-Type: application/json

{
  "username": "john.doe",
  "email": "john@example.com",
  "password": "SecurePass123!"
}

### Login
POST http://localhost:5039/api/accounts/login
Content-Type: application/json

{
  "username": "john.doe",
  "password": "SecurePass123!"
}
```

---

## 📦 Technologies

### **Backend**
- .NET 10
- ASP.NET Core (Minimal API)
- Entity Framework Core 8
- MediatR (CQRS pattern)
- FluentValidation
- JWT Bearer Authentication
- BCrypt.Net

### **Frontend**
- Angular 19
- TypeScript 5
- TailwindCSS
- SCSS
- RxJS
- Angular Router
- HTTP Interceptors

### **DevOps**
- Docker (SQL Server)
- Git
- ESLint
- Prettier

---

## 🛠️ Development

### **Backend**
```bash
cd MechaSoft.WebAPI
dotnet watch run
# API: http://localhost:5039
# Swagger: http://localhost:5039/swagger
```

### **Frontend**
```bash
cd Presentation/MechaSoft.Angular
npm install
npm start
# App: http://localhost:4200
```

### **Code Quality**
```bash
# Frontend linting
cd Presentation/MechaSoft.Angular
npm run lint

# Format code
npm run format
```

---

## 📁 Important Files

### **Configuration**
- `MechaSoft.WebAPI/appsettings.json` - Production settings
- `MechaSoft.WebAPI/appsettings.Development.json` - Development settings
- `Presentation/MechaSoft.Angular/angular.json` - Angular configuration
- `.eslintrc.json` - Linting rules
- `.prettierrc` - Code formatting rules

### **Documentation**
- `README.md` - This file
- `Presentation/MechaSoft.Angular/ESTRUTURA.md` - Angular structure
- `Presentation/MechaSoft.Angular/FLUXO_NAVEGACAO.md` - Navigation flow
- `API_TESTS.http` - API test collection

### **Scripts**
- `setup-sqlserver.sh` - Setup SQL Server (Linux)
- `start-mechasoft.sh` - Start all services (Linux)
- `stop-mechasoft.sh` - Stop all services (Linux)
- `build-mechasoft.sh` - Build entire solution

---

## 🎯 Key Components

### **Navbar** (707 lines of SCSS!)
Modern navigation with:
- Dropdown menu
- Profile with avatar
- Notifications
- Global search
- Fully responsive

### **Authentication System**
- JWT tokens with refresh
- Role-based access control
- Password hashing (BCrypt)
- Email/username validation
- Profile image upload

### **Dashboard**
- Real-time statistics
- Revenue tracking
- Low stock alerts
- Recent activity feed

---

## 🔧 Troubleshooting

### **SQL Server connection fails**
```bash
# Check if SQL Server is running
docker ps | grep sqlserver

# Restart SQL Server
docker restart mechasoft-sqlserver

# View logs
docker logs mechasoft-sqlserver
```

### **Frontend won't start**
```bash
# Clear node_modules
cd Presentation/MechaSoft.Angular
rm -rf node_modules package-lock.json
npm install
npm start
```

### **Database migration errors**
```bash
# Check migration status
dotnet ef migrations list --project MechaSoft.Data --startup-project MechaSoft.WebAPI

# Reset database (WARNING: deletes all data!)
dotnet ef database drop --project MechaSoft.Data --startup-project MechaSoft.WebAPI --force
dotnet ef database update --project MechaSoft.Data --startup-project MechaSoft.WebAPI
```

---

## 📊 Performance

### **Bundle Sizes**
- Initial Bundle: **475 KB** (117 KB gzipped)
- Lazy Chunks: **~250 KB** total
- Optimized for production with tree shaking

### **Features**
- ✅ Lazy loading modules
- ✅ Code splitting
- ✅ AOT compilation
- ✅ Production build optimization

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'feat: Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📝 Commit Convention

We follow [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: Add new feature
fix: Bug fix
docs: Documentation changes
style: Code style changes
refactor: Code refactoring
test: Add tests
chore: Build process or auxiliary tool changes
```

---

## 📄 License

This project is private and proprietary.

---

## 👥 Team

- **Developer:** Rick Sanchez
- **Project:** MechaSoft Workshop Management System
- **Version:** 3.0.0
- **Last Updated:** October 9, 2025

---

## 🔗 Links

- **Repository:** https://github.com/Rick-Sanchez4/MechaSoftApp
- **Issues:** https://github.com/Rick-Sanchez4/MechaSoftApp/issues
- **Documentation:** See `/Presentation/MechaSoft.Angular/` folder

---

## 🎯 Roadmap

### **Current (v3.0)**
- ✅ User authentication & authorization
- ✅ Customer & vehicle management
- ✅ Service order tracking
- ✅ Parts inventory
- ✅ Dashboard analytics
- ✅ Profile management
- ✅ Modern UI with animations

### **Planned**
- 🔮 Customer portal
- 🔮 Real-time notifications
- 🔮 Dark/Light theme toggle
- 🔮 PWA support
- 🔮 Mobile app
- 🔮 Advanced reporting
- 🔮 Email notifications
- 🔮 PDF invoice generation

---

## 📞 Support

For issues, questions, or suggestions, please open an issue on GitHub.

---

**Made with ❤️ using .NET 10 and Angular 19**
