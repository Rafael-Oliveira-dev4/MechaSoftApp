import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { User } from '../../../../core/models/api.models';
import { ErrorDetail } from '../../../../core/models/result.model';
import { AuthService } from '../../../../core/services/auth.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import { ProfileImageUploadComponent } from '../../../../shared/components/profile-image-upload/profile-image-upload.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ErrorMessageComponent,
    ProfileImageUploadComponent,
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  currentUser: User | null = null;
  isEditingProfile: boolean = false;
  error: ErrorDetail | null = null;
  successMessage: string | null = null;

  // Edit form
  editForm = {
    username: '',
    email: '',
  };

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      if (user?.username && user?.email) {
        this.editForm.username = user.username;
        this.editForm.email = user.email;
      }
    });
  }

  // Toggle edit mode
  toggleEditMode(): void {
    this.isEditingProfile = !this.isEditingProfile;
    if (!this.isEditingProfile && this.currentUser?.username && this.currentUser?.email) {
      // Reset form
      this.editForm.username = this.currentUser.username;
      this.editForm.email = this.currentUser.email;
    }
  }

  // Save profile changes
  saveProfile(): void {
    // TODO: Implementar atualização de perfil
    this.successMessage = 'Perfil atualizado com sucesso!';
    this.isEditingProfile = false;
    setTimeout(() => (this.successMessage = null), 3000);
  }

  // Handle profile image upload success
  onImageUploadSuccess(imageUrl: string): void {
    this.successMessage = 'Imagem de perfil atualizada com sucesso!';

    // Update current user
    if (this.currentUser) {
      this.currentUser.profileImageUrl = imageUrl;
    }

    // Clear success message after 3 seconds
    setTimeout(() => (this.successMessage = null), 3000);
  }

  // Handle profile image upload error
  onImageUploadError(errorMessage: string): void {
    this.error = {
      code: 'UPLOAD_FAILED',
      message: errorMessage,
    };

    // Clear error message after 5 seconds
    setTimeout(() => (this.error = null), 5000);
  }

  // Get role label
  getRoleLabel(): string {
    const roleMap: { [key: string]: string } = {
      Owner: 'Proprietário',
      Admin: 'Administrador',
      Employee: 'Funcionário',
      Customer: 'Cliente',
    };
    return roleMap[this.currentUser?.role || ''] || this.currentUser?.role || '';
  }

  // Format date
  formatDate(date?: Date): string {
    if (!date) return 'Nunca';
    return new Date(date).toLocaleDateString('pt-PT', {
      day: '2-digit',
      month: 'long',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  }
}
