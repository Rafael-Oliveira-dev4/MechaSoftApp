import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { User } from '../../../../core/models/api.models';
import { ErrorDetail } from '../../../../core/models/result.model';
import { AuthService } from '../../../../core/services/auth.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ErrorMessageComponent],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
})
export class SettingsComponent implements OnInit {
  currentUser: User | null = null;
  error: ErrorDetail | null = null;
  successMessage: string | null = null;

  // Settings
  settings = {
    emailNotifications: true,
    browserNotifications: false,
    marketingEmails: false,
    weeklyReports: true,
    twoFactorAuth: false,
    autoLogout: true,
    theme: 'dark', // dark, light, auto
  };

  // Password Change
  passwordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  };

  showPasswordForm: boolean = false;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  // Save settings
  saveSettings(): void {
    // TODO: Implementar salvamento de configurações
    this.successMessage = 'Configurações guardadas com sucesso!';
    setTimeout(() => (this.successMessage = null), 3000);
  }

  // Toggle password form
  togglePasswordForm(): void {
    this.showPasswordForm = !this.showPasswordForm;
    if (!this.showPasswordForm) {
      this.resetPasswordForm();
    }
  }

  // Change password
  changePassword(): void {
    // Validate passwords
    if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
      this.error = {
        code: 'PASSWORD_MISMATCH',
        message: 'As senhas não coincidem',
      };
      return;
    }

    if (this.passwordForm.newPassword.length < 8) {
      this.error = {
        code: 'PASSWORD_TOO_SHORT',
        message: 'A senha deve ter pelo menos 8 caracteres',
      };
      return;
    }

    // Validate password complexity
    const password = this.passwordForm.newPassword;
    if (!/[A-Z]/.test(password) || !/[a-z]/.test(password) || !/[0-9]/.test(password) || !/[\W_]/.test(password)) {
      this.error = {
        code: 'PASSWORD_TOO_WEAK',
        message: 'A senha deve conter maiúsculas, minúsculas, números e caracteres especiais',
      };
      return;
    }

    if (!this.currentUser?.id) {
      this.error = {
        code: 'USER_NOT_FOUND',
        message: 'Utilizador não encontrado',
      };
      return;
    }

    // Call API to change password
    this.authService.changePassword(
      this.currentUser.id,
      this.passwordForm.currentPassword,
      this.passwordForm.newPassword
    ).subscribe(result => {
      if (result.isSuccess) {
        this.successMessage = 'Senha alterada com sucesso!';
        this.resetPasswordForm();
        this.showPasswordForm = false;
        setTimeout(() => (this.successMessage = null), 3000);
        this.error = null;
      } else {
        this.error = result.error || {
          code: 'CHANGE_PASSWORD_FAILED',
          message: 'Erro ao alterar senha'
        };
      }
    });
  }

  // Reset password form
  private resetPasswordForm(): void {
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    };
  }

  // Export data
  exportData(): void {
    // TODO: Implementar exportação de dados
    this.successMessage = 'Dados exportados com sucesso!';
    setTimeout(() => (this.successMessage = null), 3000);
  }

  // Delete account (dangerous action)
  deleteAccount(): void {
    const confirmation = confirm(
      'Tem a certeza que deseja eliminar a sua conta? Esta ação é irreversível!'
    );
    if (confirmation) {
      // TODO: Implementar eliminação de conta
      alert('Funcionalidade em desenvolvimento');
    }
  }
}
