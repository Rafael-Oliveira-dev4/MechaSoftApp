import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ErrorDetail } from '../../../core/models/result.model';
import { AuthService } from '../../../core/services/auth.service';
import { emailAvailabilityValidator } from '../../../core/validators/email-availability.validator';
import { usernameAvailabilityValidator } from '../../../core/validators/username-availability.validator';
import { ErrorMessageComponent } from '../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, ErrorMessageComponent],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm: FormGroup;
  error: ErrorDetail | null = null;
  isLoading: boolean = false;
  successMessage: string | null = null;
  showPassword: boolean = false;
  showConfirmPassword: boolean = false;
  fieldErrors: { [key: string]: string } = {};
  usernameSuggestions: string[] = [];
  isCheckingEmail: boolean = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group(
      {
        username: [
          '',
          {
            validators: [
              Validators.required,
              Validators.minLength(3),
              Validators.maxLength(50),
              Validators.pattern(/^[a-zA-Z0-9_]+$/),
            ],
            asyncValidators: [usernameAvailabilityValidator(this.authService)],
            updateOn: 'change', // Executa a validação a cada mudança
          },
        ],
        email: [
          '',
          [Validators.required, Validators.email],
          [emailAvailabilityValidator(this.authService)], // Async validator
        ],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/),
          ],
        ],
        confirmPassword: ['', [Validators.required]],
      },
      { validators: this.passwordMatchValidator }
    );

    // Monitorar erros de username para buscar sugestões
    this.registerForm.get('username')?.statusChanges.subscribe(status => {
      const control = this.registerForm.get('username');
      if (status === 'INVALID' && control?.hasError('usernameTaken')) {
        // Buscar sugestões quando username já existe
        this.authService.suggestUsername(control.value).subscribe(suggestions => {
          this.usernameSuggestions = suggestions;
        });
      } else {
        this.usernameSuggestions = [];
      }
    });

    // Monitorar o status de validação do email
    this.registerForm.get('email')?.statusChanges.subscribe((status: string) => {
      this.isCheckingEmail = status === 'PENDING';
    });
  }

  // Aplicar sugestão de username
  applySuggestion(suggestion: string): void {
    this.registerForm.get('username')?.setValue(suggestion);
    this.usernameSuggestions = [];
  }

  // Validador customizado para confirmar password
  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }

    return null;
  }

  // Submeter registo
  onSubmit(): void {
    // Bloquear submit se ainda está validando (PENDING)
    if (this.registerForm.pending) {
      return;
    }

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.error = null;
    this.successMessage = null;
    this.fieldErrors = {}; // Clear previous field errors

    const { username, email, password, confirmPassword } = this.registerForm.value;

    this.authService
      .register({
        username,
        email,
        password,
        confirmPassword,
        role: 'Customer', // Default role para novos registos
      })
      .subscribe({
        next: result => {
          this.isLoading = false;
          if (result.isSuccess) {
            this.successMessage = 'Registo efetuado com sucesso! A redirecionar para login...';
            setTimeout(() => {
              this.router.navigate(['/login']);
            }, 2000);
          } else {
            this.error = result.error || null;
            this.handleFieldErrors(result.error);
          }
        },
        error: err => {
          this.isLoading = false;
          this.error = err;
          this.handleFieldErrors(err);
        },
      });
  }

  // Helper para validação
  isFieldInvalid(fieldName: string): boolean {
    // Show error if there's a backend error for this field
    if (this.fieldErrors[fieldName]) {
      return true;
    }

    // Otherwise check form validation
    const field = this.registerForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    // Check for backend errors first
    if (this.fieldErrors[fieldName]) {
      return this.fieldErrors[fieldName];
    }

    // Then check for form validation errors
    const field = this.registerForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'Campo obrigatório';
    if (field.errors['minlength'])
      return `Mínimo ${field.errors['minlength'].requiredLength} caracteres`;
    if (field.errors['maxlength'])
      return `Máximo ${field.errors['maxlength'].requiredLength} caracteres`;
    if (field.errors['email']) return 'E-mail inválido';
    if (field.errors['emailTaken']) return 'Este e-mail já está registado';
    if (field.errors['passwordMismatch']) return 'As palavras-passe não coincidem';
    if (field.errors['usernameTaken']) return 'Este nome de utilizador já está em uso';
    if (field.errors['pattern']) {
      if (fieldName === 'username') {
        return 'Apenas letras, números e underscore (_)';
      }
      if (fieldName === 'password') {
        return 'Deve ter maiúscula, minúscula, número e carácter especial';
      }
    }

    return 'Campo inválido';
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  getPasswordStrength(): { strength: string; color: string; width: string } {
    const password = this.registerForm.get('password')?.value || '';
    if (password.length === 0) {
      return { strength: '', color: '', width: '0%' };
    }

    let score = 0;
    if (password.length >= 8) score++;
    if (/[a-z]/.test(password)) score++;
    if (/[A-Z]/.test(password)) score++;
    if (/[0-9]/.test(password)) score++;
    if (/[^a-zA-Z0-9]/.test(password)) score++;

    if (score <= 2) {
      return { strength: 'Fraca', color: '#ef4444', width: '33%' };
    } else if (score <= 4) {
      return { strength: 'Média', color: '#f59e0b', width: '66%' };
    } else {
      return { strength: 'Forte', color: '#10b981', width: '100%' };
    }
  }

  getPasswordRequirements(): Array<{ text: string; met: boolean }> {
    const password = this.registerForm.get('password')?.value || '';
    return [
      { text: 'Mínimo 8 caracteres', met: password.length >= 8 },
      { text: 'Uma letra maiúscula', met: /[A-Z]/.test(password) },
      { text: 'Uma letra minúscula', met: /[a-z]/.test(password) },
      { text: 'Um número', met: /[0-9]/.test(password) },
      { text: 'Um carácter especial (!@#$%...)', met: /[^a-zA-Z0-9]/.test(password) },
    ];
  }

  // Handle field-specific errors
  private handleFieldErrors(error: any): void {
    if (!error) return;

    // Check for specific error codes
    if (error.code === 'UsernameAlreadyExists') {
      this.fieldErrors['username'] = 'Este nome de utilizador já está em uso';
    } else if (error.code === 'EmailAlreadyExists') {
      this.fieldErrors['email'] = 'Este e-mail já está registado';
    }
  }
}
