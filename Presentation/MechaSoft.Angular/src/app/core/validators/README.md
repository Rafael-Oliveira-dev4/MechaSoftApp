# Validadores Customizados - MechaSoft

Este diretório contém validadores customizados para formulários Angular.

## Validadores Disponíveis

### 1. NIF Validator (`nif.validator.ts`)

Valida Número de Identificação Fiscal (NIF) português.

**Uso:**
```typescript
import { nifValidator } from '@core/validators';

this.form = this.fb.group({
  nif: ['', [nifValidator()]]
});
```

**Funções auxiliares:**
- `formatNif(nif: string)`: Formata NIF para exibição (XXX XXX XXX)

### 2. Phone Portuguese Validator (`phone-pt.validator.ts`)

Valida telefones portugueses (fixos e móveis).

**Uso:**
```typescript
import { phonePortugueseValidator, mobilePhonePortugueseValidator } from '@core/validators';

this.form = this.fb.group({
  phone: ['', [phonePortugueseValidator()]],
  mobile: ['', [mobilePhonePortugueseValidator()]]
});
```

**Funções auxiliares:**
- `formatPhonePortuguese(phone: string)`: Formata telefone para exibição

### 3. Email Availability Validator (`email-availability.validator.ts`)

Valida se o email já está em uso (async validator).

**Uso:**
```typescript
import { emailAvailabilityValidator } from '@core/validators';

this.form = this.fb.group({
  email: ['', [Validators.required, Validators.email], [emailAvailabilityValidator(this.authService)]]
});
```

### 4. Username Availability Validator (`username-availability.validator.ts`)

Valida se o username já está em uso (async validator).

**Uso:**
```typescript
import { usernameAvailabilityValidator } from '@core/validators';

this.form = this.fb.group({
  username: ['', [Validators.required], [usernameAvailabilityValidator(this.authService)]]
});
```

## Componentes de Validação

### FormFieldError Component

Componente standalone para exibir erros de validação.

**Uso:**
```html
<input 
  type="text" 
  formControlName="nif"
  class="form-input"
  placeholder="NIF"
/>
<app-form-field-error 
  [control]="form.get('nif')" 
  fieldName="NIF"
/>
```

**Com mensagens customizadas:**
```html
<app-form-field-error 
  [control]="form.get('email')" 
  fieldName="Email"
  [customMessages]="{
    'emailTaken': 'Este email já está registado no sistema',
    'required': 'Por favor, insira o seu email'
  }"
/>
```

### RealTimeValidation Directive

Diretiva para adicionar validação visual em tempo real.

**Uso:**
```html
<input 
  type="text" 
  formControlName="phone"
  class="form-input"
  appRealTimeValidation
  placeholder="Telefone"
/>
```

**Opções:**
- `showValidState`: Mostrar estado válido (verde) - padrão: `true`
- `validateOnBlur`: Validar apenas no blur - padrão: `false`

```html
<input 
  type="text" 
  formControlName="phone"
  class="form-input"
  appRealTimeValidation
  [showValidState]="true"
  [validateOnBlur]="false"
/>
```

## Exemplo Completo

```typescript
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { nifValidator, phonePortugueseValidator } from '@core/validators';

@Component({
  selector: 'app-customer-form',
  template: `
    <form [formGroup]="form" (ngSubmit)="onSubmit()">
      <!-- Nome -->
      <div class="form-group">
        <label>Nome</label>
        <input 
          type="text" 
          formControlName="name"
          class="form-input"
          appRealTimeValidation
          placeholder="Nome completo"
        />
        <app-form-field-error 
          [control]="form.get('name')" 
          fieldName="Nome"
        />
      </div>

      <!-- NIF -->
      <div class="form-group">
        <label>NIF</label>
        <input 
          type="text" 
          formControlName="nif"
          class="form-input"
          appRealTimeValidation
          placeholder="123456789"
          maxlength="9"
        />
        <app-form-field-error 
          [control]="form.get('nif')" 
          fieldName="NIF"
        />
      </div>

      <!-- Telefone -->
      <div class="form-group">
        <label>Telefone</label>
        <input 
          type="tel" 
          formControlName="phone"
          class="form-input"
          appRealTimeValidation
          placeholder="912345678"
          maxlength="9"
        />
        <app-form-field-error 
          [control]="form.get('phone')" 
          fieldName="Telefone"
        />
      </div>

      <button type="submit" class="btn-primary" [disabled]="form.invalid">
        Guardar
      </button>
    </form>
  `
})
export class CustomerFormComponent {
  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      nif: ['', [nifValidator()]],
      phone: ['', [Validators.required, phonePortugueseValidator()]]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      console.log(this.form.value);
    }
  }
}
```

## Classes CSS de Validação

As classes seguintes são aplicadas automaticamente pela diretiva `appRealTimeValidation`:

- `.field-valid`: Campo válido (borda verde)
- `.field-invalid`: Campo inválido (borda vermelha)

Estilos definidos em `styles/_ui.scss`.

