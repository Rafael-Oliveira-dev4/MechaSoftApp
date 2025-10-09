# 🎨 MechaSoft Angular Frontend

Modern Angular 19 frontend for the MechaSoft Workshop Management System.

---

## 🚀 Quick Start

```bash
# Install dependencies
npm install

# Start development server
npm start
# Open http://localhost:4200

# Build for production
npm run build
# Output: dist/MechaSoft.Angular

# Run linter
npm run lint

# Format code
npm run format
```

---

## 📁 Project Structure

See `ESTRUTURA.md` for detailed architecture documentation.

```
/src/app/
├── core/                   # Singleton services, guards, models
│   ├── guards/            # Authentication & role guards
│   ├── interceptors/      # HTTP interceptors
│   ├── models/            # Data models & interfaces
│   ├── services/          # API services
│   └── validators/        # Async validators
│
├── shared/                # Reusable components
│   ├── components/
│   │   ├── navbar/       # Main navigation (707 lines CSS!)
│   │   ├── profile-image-upload/
│   │   ├── error/
│   │   ├── error-message/
│   │   ├── loading-spinner/
│   │   └── page-header/
│   └── shared.module.ts
│
└── components/            # Feature modules
    ├── auth/
    │   ├── login/
    │   └── register/
    ├── landing/           # Public landing page
    ├── front-office/      # Management system
    │   └── pages/
    │       ├── dashboard/
    │       ├── profile/
    │       ├── settings/
    │       ├── customers/
    │       ├── vehicles/
    │       ├── service-orders/
    │       ├── inspections/
    │       ├── services/
    │       └── parts/
    └── back-office/       # Admin panel (future)
```

---

## 🗺️ Routes

See `FLUXO_NAVEGACAO.md` for detailed routing documentation.

### **Public**
- `/` - Landing page
- `/login` - Login
- `/register` - Registration
- `/404` - Error page

### **Authenticated** (`/app/*`)
- `/app/dashboard` - Main dashboard
- `/app/profile` - User profile
- `/app/settings` - User settings
- `/app/customers` - Customer management
- `/app/vehicles` - Vehicle management
- `/app/service-orders` - Service orders
- `/app/inspections` - Inspections
- `/app/services` - Service catalog
- `/app/parts` - Parts inventory

---

## 🎨 Design System

### **Colors**
- **Primary:** Cyan → Blue gradient
- **Secondary:** Blue → Indigo gradient
- **Success:** Emerald → Teal gradient
- **Warning:** Amber → Orange gradient
- **Danger:** Red → Pink gradient
- **Info:** Purple → Violet gradient

### **Animations**
- `animate-float` - Smooth vertical movement
- `animate-pulse-glow` - Pulsing glow effect
- `animate-gradient` - Moving gradients
- `animate-marquee` - Infinite scroll

### **Effects**
- Glassmorphism (backdrop blur)
- 3D card transformations
- Smooth hover effects
- Gradient backgrounds

---

## 🔧 Configuration

### **Environment Variables**
- `src/environments/environment.ts` - Production
- `src/environments/environment.development.ts` - Development

### **API Base URL**
Default: `http://localhost:5039/api`

---

## 📦 Key Dependencies

- **@angular/core:** ^19.0.0
- **@angular/router:** ^19.0.0
- **rxjs:** ^7.8.0
- **tailwindcss:** ^3.4.0

---

## 🎯 Features

### **Authentication**
- JWT token management
- Refresh token flow
- Role-based access control
- Route guards

### **User Management**
- Profile editing
- Image upload with preview
- Settings management
- Password change

### **Workshop Features**
- Customer CRUD
- Vehicle tracking
- Service order management
- Inspection scheduling
- Parts inventory
- Service catalog

### **UI/UX**
- Responsive design
- Dark theme
- Smooth animations
- Loading states
- Error handling
- Form validation

---

## 📚 Documentation

- **ESTRUTURA.md** - Project architecture and structure
- **FLUXO_NAVEGACAO.md** - Navigation flow and routing
- **README.md** - This file

---

## 🔨 Build & Deploy

### **Development Build**
```bash
npm start
```

### **Production Build**
```bash
npm run build
# Output: dist/MechaSoft.Angular (475 KB initial bundle)
```

### **Build Configuration**
- **Budget Limits:**
  - Initial bundle: 1 MB
  - Component styles: 100 KB
- **Optimizations:**
  - Tree shaking
  - Minification
  - AOT compilation
  - Lazy loading

---

## 🐛 Known Issues

None at the moment! 🎉

---

## 📝 Notes

- All services use `providedIn: 'root'` for singleton pattern
- Lazy loading implemented for all feature modules
- Guards protect routes based on authentication and roles
- HTTP interceptors handle auth tokens, errors, and loading states

---

**Version:** 3.0.0  
**Last Updated:** October 9, 2025  
**Angular Version:** 19  
**Node Version:** 18+
