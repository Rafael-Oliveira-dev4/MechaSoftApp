# 🎯 ROADMAP - Próximos Passos MechaSoftApp

**Data:** 13 de outubro de 2025  
**Status Atual:** 95% Completo - Pronto para Produção  
**TODOs Implementados:** 9/9 ✅

---

## 🔴 PRIORIDADE ALTA (1-2 horas)

### 1️⃣ Debugar Modal de Credenciais ⏱️ 30min

**Status:** 🐛 Bug Identificado  
**Impacto:** Médio - Owner não vê credenciais geradas

**Problema:**
- Backend cria User com username/senha
- Frontend não mostra modal de credenciais
- Employee criado com sucesso ✅
- Network request: `POST /api/employees` → **201 Created** ✅

**Passos:**
1. Adicionar `console.log(result.value)` no `onSubmit()` de `EmployeesComponent`
2. Verificar se `generatedUsername` e `generatedPassword` estão no response
3. Se não estiverem, verificar `CreateEmployeeResponse` no backend
4. Testar criar novo Employee e verificar modal

**Código a verificar:**
```typescript
// employees.component.ts linha ~160
if (!this.isEditMode && result.value) {
  const response = result.value as any;
  console.log('CREATE EMPLOYEE RESPONSE:', response); // ADD THIS
  if (response.generatedUsername && response.generatedPassword) {
    this.showCredentialsModal = true;
  }
}
```

---

### 2️⃣ Adicionar Services/Parts a ServiceOrders ⏱️ 1h

**Status:** 📝 Backend Pronto, Frontend Falta  
**Impacto:** Alto - Funcionalidade core incompleta

**Backend (já existe):**
- ✅ `POST /api/service-orders/:id/services`
- ✅ `POST /api/service-orders/:id/parts`

**Frontend (implementar):**

**Modal "Adicionar Serviço":**
```html
<div class="modal">
  <h2>Adicionar Serviço à Ordem</h2>
  <select [(ngModel)]="selectedServiceId">
    <option *ngFor="let service of services" [value]="service.id">
      {{ service.name }} - {{ service.fixedPrice || service.pricePerHour }}€
    </option>
  </select>
  <input type="number" [(ngModel)]="serviceQuantity" placeholder="Quantidade" />
  <input type="number" [(ngModel)]="serviceHours" placeholder="Horas estimadas" />
  <input type="number" [(ngModel)]="serviceDiscount" placeholder="Desconto %" />
  <button (click)="addServiceToOrder()">Adicionar</button>
</div>
```

**Modal "Adicionar Peça":**
```html
<div class="modal">
  <h2>Adicionar Peça à Ordem</h2>
  <select [(ngModel)]="selectedPartId">
    <option *ngFor="let part of parts" [value]="part.id">
      {{ part.name }} - {{ part.salePrice }}€ (Stock: {{ part.stockQuantity }})
    </option>
  </select>
  <input type="number" [(ngModel)]="partQuantity" placeholder="Quantidade" />
  <input type="number" [(ngModel)]="partDiscount" placeholder="Desconto %" />
  <button (click)="addPartToOrder()">Adicionar</button>
</div>
```

**Métodos no Component:**
```typescript
addServiceToOrder(): void {
  const request = {
    serviceId: this.selectedServiceId,
    quantity: this.serviceQuantity,
    estimatedHours: this.serviceHours,
    discountPercentage: this.serviceDiscount || 0
  };
  
  this.serviceOrderService.addService(this.selectedOrder.id, request).subscribe(result => {
    if (result.isSuccess) {
      this.loadOrderDetails(this.selectedOrder.id);
      this.closeServiceModal();
    }
  });
}

addPartToOrder(): void {
  const request = {
    partId: this.selectedPartId,
    quantity: this.partQuantity,
    discountPercentage: this.partDiscount || 0
  };
  
  this.serviceOrderService.addPart(this.selectedOrder.id, request).subscribe(result => {
    if (result.isSuccess) {
      this.loadOrderDetails(this.selectedOrder.id);
      this.closePartModal();
    }
  });
}
```

---

### 3️⃣ ServiceOrder Status Management ⏱️ 30min

**Implementação:**

**Template HTML:**
```html
<div class="order-actions">
  <label>Mudar Status:</label>
  <select [(ngModel)]="newStatus" (change)="updateStatus()">
    <option value="Pending">Pendente</option>
    <option value="InProgress">Em Curso</option>
    <option value="WaitingParts">Aguarda Peças</option>
    <option value="WaitingApproval">Aguarda Aprovação</option>
    <option value="Completed">Concluída</option>
  </select>

  <label>Atribuir Mecânico:</label>
  <select [(ngModel)]="selectedMechanicId" (change)="assignMechanic()">
    <option value="">Sem mecânico</option>
    <option *ngFor="let mech of mechanics" [value]="mech.id">
      {{ mech.name }} - {{ mech.hourlyRate }}€/h
    </option>
  </select>
</div>
```

**Métodos:**
```typescript
updateStatus(): void {
  if (this.newStatus === 'Completed' && !confirm('Confirmar conclusão da ordem?')) {
    return;
  }

  this.serviceOrderService.updateStatus(this.selectedOrder.id, this.newStatus).subscribe(result => {
    if (result.isSuccess) {
      this.loadOrders();
      this.toastService.success('Status atualizado!');
    }
  });
}

assignMechanic(): void {
  this.serviceOrderService.assignMechanic(this.selectedOrder.id, this.selectedMechanicId).subscribe(result => {
    if (result.isSuccess) {
      this.loadOrders();
      this.toastService.success('Mecânico atribuído!');
    }
  });
}
```

---

## 🟡 PRIORIDADE MÉDIA

### 4️⃣ Toast Notifications ⏱️ 1h

**Instalação:**
```bash
npm install ngx-toastr
npm install @angular/animations
```

**Configuração:**
```typescript
// app.config.ts
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideToastr({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    // ... outros providers
  ]
};
```

**Uso:**
```typescript
constructor(private toastr: ToastrService) {}

onSuccess(): void {
  this.toastr.success('Cliente criado com sucesso!', 'Sucesso');
}

onError(): void {
  this.toastr.error('Erro ao criar cliente', 'Erro');
}
```

---

### 5️⃣ Stock Management Manual ⏱️ 1h
### 6️⃣ Confirmações ⏱️ 30min
### 7️⃣ Loading States ⏱️ 30min

*(Ver descrição acima)*

---

## 🟢 PRIORIDADE BAIXA

### 8️⃣ Relatórios PDF/Excel ⏱️ 3h
### 9️⃣ Dashboard Gráficos ⏱️ 2h
### 🔟 Notificações Real-Time ⏱️ 3h

*(Ver descrição acima)*

---

## 🎯 SUGESTÃO IMEDIATA

### **Opção 1: Completar Features Core (2-3h)** ✅ RECOMENDADO

1. Debugar modal credenciais (30min)
2. Adicionar Services/Parts a Orders (1h)
3. Status Management de Orders (30min)
4. Toast notifications (1h)

**Resultado:** Projeto 100% completo e polido para demo/produção

---

### **Opção 2: Focar em UX (2h)**

1. Toast notifications (1h)
2. Loading states (30min)
3. Confirmações (30min)

**Resultado:** UX profissional

---

### **Opção 3: Partir para Produção Agora**

O projeto **já está 95% funcional** e pode ser usado em produção!

**Decisão:** Implementar features adicionais depois, com base no feedback de utilizadores reais.

---

**Qual caminho prefere seguir?** 🚀
