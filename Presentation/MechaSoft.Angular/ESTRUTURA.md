# 📁 Estrutura do Projeto Angular - MechaSoft

## ✅ Estrutura Organizada (Clean Architecture)

```
/src/app/
├── core/                              # Serviços singleton, guards, interceptors, models
│   ├── guards/
│   │   ├── auth.guard.ts             # Proteção de rotas autenticadas
│   │   └── role.guard.ts             # Proteção baseada em roles
│   │
│   ├── interceptors/
│   │   ├── auth.interceptor.ts       # Adiciona token JWT às requisições
│   │   ├── error.interceptor.ts      # Trata erros HTTP globalmente
│   │   └── loading.interceptor.ts    # Controla estado de loading
│   │
│   ├── models/                        # ✅ ÚNICO local para models
│   │   ├── api.models.ts
│   │   ├── common.ts
│   │   ├── customer.model.ts
│   │   ├── dashboard.model.ts
│   │   ├── employee.model.ts
│   │   ├── enums.ts
│   │   ├── inspection.model.ts
│   │   ├── part.model.ts
│   │   ├── result.model.ts
│   │   ├── service-item.model.ts
│   │   ├── service-order.model.ts
│   │   ├── service.model.ts
│   │   ├── user.model.ts
│   │   ├── vehicle.model.ts
│   │   └── index.ts
│   │
│   └── services/                      # ✅ ÚNICO local para services
│       ├── api-config.service.ts
│       ├── auth.service.ts
│       ├── customer.service.ts
│       ├── dashboard.service.ts
│       ├── employee.service.ts
│       ├── inspection.service.ts
│       ├── loading.service.ts
│       ├── mechanic-service.service.ts
│       ├── part.service.ts
│       ├── service-order.service.ts
│       ├── user.service.ts            # ✅ Movido para cá
│       ├── vehicle.service.ts
│       └── index.ts
│
├── shared/                            # Componentes/módulos compartilhados
│   ├── components/
│   │   ├── error/                     # ✅ Movido de components/common/
│   │   │   └── error.component.ts
│   │   ├── error-message/
│   │   │   ├── error-message.component.html
│   │   │   ├── error-message.component.scss
│   │   │   └── error-message.component.ts
│   │   ├── loading-spinner/
│   │   │   ├── loading-spinner.component.html
│   │   │   ├── loading-spinner.component.scss
│   │   │   └── loading-spinner.component.ts
│   │   └── page-header/               # ✅ Movido de front-office/components/
│   │       ├── page-header.component.html
│   │       ├── page-header.component.scss
│   │       └── page-header.component.ts
│   │
│   └── shared.module.ts               # ✅ Movido da raiz de /app
│
├── components/                        # Feature modules organizados por área
│   ├── auth/
│   │   └── login/
│   │       ├── login.component.html
│   │       ├── login.component.scss
│   │       └── login.component.ts
│   │
│   ├── landing/                       # ✅ Landing page pública
│   │   ├── landing.component.html
│   │   ├── landing.component.scss
│   │   ├── landing.component.ts
│   │   └── landing.module.ts
│   │
│   ├── front-office/                  # Sistema de Gestão (para funcionários)
│   │   ├── layout/
│   │   │   ├── front-office-layout.component.html
│   │   │   ├── front-office-layout.component.scss
│   │   │   └── front-office-layout.component.ts
│   │   │
│   │   ├── pages/
│   │   │   ├── dashboard/
│   │   │   ├── customers/
│   │   │   ├── vehicles/
│   │   │   ├── service-orders/
│   │   │   ├── inspections/
│   │   │   ├── services/
│   │   │   ├── parts/
│   │   │   └── home/
│   │   │
│   │   ├── front-office-routing.module.ts
│   │   └── front-office.module.ts
│   │
│   └── back-office/                   # Back-office (futuro)
│       ├── back-office-layout.component.ts  # ✅ Renomeado de main.component.ts
│       ├── back-office-routing.module.ts
│       └── back-office.module.ts
│
├── app.component.ts                   # ✅ Renomeado de app.ts
├── app.module.ts                      # ✅ Renomeado de app-module.ts
├── app-routing.module.ts              # ✅ Renomeado de app-routing-module.ts
├── app.module.server.ts
├── app.routes.server.ts
├── app.scss
└── app.spec.ts
```

## 🎯 Rotas da Aplicação

```
/                      → Landing Page (pública)
/login                 → Login (público)
/app                   → Sistema de Gestão (autenticado)
  ├─ /app/dashboard
  ├─ /app/customers
  ├─ /app/vehicles
  ├─ /app/service-orders
  ├─ /app/inspections
  ├─ /app/services
  └─ /app/parts
/admin                 → Back-office (futuro)
/404                   → Página de erro
```

## 📋 Mudanças Realizadas

### ✅ Arquivos Movidos:
1. `user.service.ts`: `/services/` → `/core/services/`
2. `shared.module.ts`: `/app/` → `/shared/`
3. `ErrorComponent`: `/components/common/error/` → `/shared/components/error/`
4. `PageHeaderComponent`: `/front-office/components/page-header/` → `/shared/components/page-header/`

### ✅ Arquivos Renomeados:
1. `app.ts` → `app.component.ts`
2. `app-module.ts` → `app.module.ts`
3. `app-routing-module.ts` → `app-routing.module.ts`
4. `main.component.ts` → `back-office-layout.component.ts`

### ✅ Arquivos/Pastas Removidos:
1. `/app/services/` - pasta vazia após mover user.service.ts
2. `/app/models/` - pasta vazia
3. `/app/components/common/` - pasta vazia após mover ErrorComponent
4. `home-simple.component.html` - arquivo órfão sem component

### ✅ Imports Atualizados:
- `app.module.ts`
- `app-routing.module.ts`
- `app.module.server.ts`
- `app.spec.ts`
- `main.ts`
- `front-office.module.ts`
- `back-office.module.ts`

## 📐 Princípios Aplicados

### **Single Responsibility (Responsabilidade Única)**
- Cada pasta tem uma responsabilidade clara
- Services em `/core/services/`
- Models em `/core/models/`
- Components compartilhados em `/shared/`

### **DRY (Don't Repeat Yourself)**
- Sem duplicação de pastas (services, models)
- Components reutilizáveis centralizados em `/shared/`

### **Convenções Angular**
- Nomenclatura de arquivos seguindo padrão oficial
- Estrutura modular clara
- Separação de concerns

### **Clean Architecture**
- Core: Lógica de negócio e serviços
- Shared: Componentes reutilizáveis
- Components: Features organizadas por domínio

## 🚀 Benefícios

✅ **Manutenibilidade**: Fácil localizar e modificar código  
✅ **Escalabilidade**: Estrutura preparada para crescimento  
✅ **Consistência**: Padrão único em todo o projeto  
✅ **Clareza**: Organização intuitiva para novos desenvolvedores  
✅ **Boas Práticas**: Segue convenções oficiais do Angular  

## 📝 Notas Importantes

- **Core Module**: Nunca deve ser importado em feature modules (só no AppModule)
- **Shared Module**: Pode ser importado em qualquer feature module
- **Feature Modules**: Devem ser lazy-loaded quando possível (já implementado)
- **Services**: Todos com `providedIn: 'root'` para singleton
- **Components**: Standalone quando possível para melhor tree-shaking

---

**Data da Reorganização**: 9 de Outubro de 2025  
**Versão**: 1.0.0  
**Status**: ✅ Completo - Sem erros de linter

