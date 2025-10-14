import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ErrorDetail } from '../../../core/models/result.model';
import { AuthService } from '../../../core/services/auth.service';
import { ErrorMessageComponent } from '../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, ErrorMessageComponent],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginForm: FormGroup;
  error: ErrorDetail | null = null;
  isLoading: boolean = false;
  showPassword: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  // Submeter login
  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.authService.login(this.loginForm.value).subscribe({
      next: result => {
        this.isLoading = false;

        if (result.isSuccess) {
          if (result.value) {
            // Redirect baseado na role
            const role = result.value.role;
            if (role === 'Customer') {
              // Clientes vão para o sistema de gestão (como todos os utilizadores)
              this.router.navigate(['/admin/dashboard']);
            } else {
              // Employee, Admin, Owner → Sistema de gestão
              this.router.navigate(['/admin/dashboard']);
            }
          } else {
            // Success but no value - shouldn't happen
            this.error = {
              code: 'UNKNOWN_ERROR',
              message: 'Login bem-sucedido mas sem dados',
              statusCode: 200,
            };
          }
        } else {
          // Login failed
          this.error = result.error || {
            code: 'UNKNOWN_ERROR',
            message: 'Erro desconhecido',
            statusCode: 0,
          };
        }
      },
      error: err => {
        this.isLoading = false;
        this.error = err || { code: 'NETWORK_ERROR', message: 'Erro de rede', statusCode: 0 };
      },
    });
  }

  // Helper para validação
  isFieldInvalid(fieldName: string): boolean {
    const field = this.loginForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.loginForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'Campo obrigatório';
    if (field.errors['minlength'])
      return `Mínimo ${field.errors['minlength'].requiredLength} caracteres`;

    return 'Campo inválido';
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
}
