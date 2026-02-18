import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-form-field-error',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (shouldShowError()) {
      <div class="error-message">
        <svg xmlns="http://www.w3.org/2000/svg" class="error-icon" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z" clip-rule="evenodd" />
        </svg>
        <span>{{ getErrorMessage() }}</span>
      </div>
    }
  `,
  styles: [`
    .error-message {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      margin-top: 0.25rem;
      font-size: 0.875rem;
      color: #ef4444;
      animation: slideDown 0.2s ease-out;
    }

    .error-icon {
      width: 1rem;
      height: 1rem;
      flex-shrink: 0;
    }

    @keyframes slideDown {
      from {
        opacity: 0;
        transform: translateY(-0.5rem);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }
  `]
})
export class FormFieldErrorComponent {
  @Input() control: AbstractControl | null = null;
  @Input() fieldName: string = 'Campo';
  @Input() customMessages: { [key: string]: string } = {};

  shouldShowError(): boolean {
    return !!(this.control && this.control.invalid && (this.control.dirty || this.control.touched));
  }

  getErrorMessage(): string {
    if (!this.control || !this.control.errors) {
      return '';
    }

    const errors = this.control.errors;

    // Verifica se há mensagem customizada
    const errorKey = Object.keys(errors)[0];
    if (this.customMessages[errorKey]) {
      return this.customMessages[errorKey];
    }

    // Mensagens de erro padrão
    if (errors['required']) {
      return `${this.fieldName} é obrigatório`;
    }

    if (errors['email']) {
      return 'Email inválido';
    }

    if (errors['minlength']) {
      const requiredLength = errors['minlength'].requiredLength;
      return `${this.fieldName} deve ter pelo menos ${requiredLength} caracteres`;
    }

    if (errors['maxlength']) {
      const requiredLength = errors['maxlength'].requiredLength;
      return `${this.fieldName} não pode exceder ${requiredLength} caracteres`;
    }

    if (errors['min']) {
      const min = errors['min'].min;
      return `${this.fieldName} deve ser maior ou igual a ${min}`;
    }

    if (errors['max']) {
      const max = errors['max'].max;
      return `${this.fieldName} deve ser menor ou igual a ${max}`;
    }

    if (errors['pattern']) {
      return `${this.fieldName} está em formato inválido`;
    }

    if (errors['invalidNif']) {
      return errors['invalidNif'].message || 'NIF inválido';
    }

    if (errors['invalidPhone']) {
      return errors['invalidPhone'].message || 'Telefone inválido';
    }

    if (errors['invalidMobile']) {
      return errors['invalidMobile'].message || 'Telemóvel inválido';
    }

    if (errors['emailTaken']) {
      return 'Este email já está em uso';
    }

    if (errors['usernameTaken']) {
      return 'Este nome de usuário já está em uso';
    }

    if (errors['passwordMismatch']) {
      return 'As senhas não coincidem';
    }

    // Se houver uma mensagem no erro, retorne-a
    if (typeof errors[errorKey] === 'object' && errors[errorKey].message) {
      return errors[errorKey].message;
    }

    // Mensagem genérica
    return `${this.fieldName} é inválido`;
  }
}

