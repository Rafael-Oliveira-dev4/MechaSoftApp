import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { User } from '../../../../core/models/api.models';
import { ErrorDetail } from '../../../../core/models/result.model';
import { AuthService } from '../../../../core/services/auth.service';
import { CustomerService, CompleteCustomerProfileRequest } from '../../../../core/services/customer.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import { ProfileImageUploadComponent } from '../../../../shared/components/profile-image-upload/profile-image-upload.component';
import { CustomerStatusCardComponent } from '../../components/customer-status-card/customer-status-card.component';
import { CustomerProfileFormComponent, CustomerProfileFormData } from '../../components/customer-profile-form/customer-profile-form.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ErrorMessageComponent,
    ProfileImageUploadComponent,
    CustomerStatusCardComponent,
    CustomerProfileFormComponent,
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  currentUser: User | null = null;
  isEditingProfile: boolean = false;
  isEditingCustomerProfile: boolean = false;
  isLoadingCustomerProfile: boolean = false;
  error: ErrorDetail | null = null;
  successMessage: string | null = null;

  // Edit form
  editForm = {
    username: '',
    email: '',
  };

  constructor(
    private authService: AuthService,
    private customerService: CustomerService
  ) {}

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

  // Check if user is customer
  isCustomer(): boolean {
    return this.currentUser?.role === 'Customer';
  }

  // Check if user has customer profile
  hasCustomerProfile(): boolean {
    return !!this.currentUser?.customerId;
  }

  // Toggle customer profile edit mode
  toggleCustomerProfileEdit(): void {
    this.isEditingCustomerProfile = !this.isEditingCustomerProfile;
  }

  // Handle customer profile form submit
  onCustomerProfileSubmit(formData: CustomerProfileFormData): void {
    if (!this.currentUser?.id) {
      this.error = { code: 'NO_USER', message: 'Utilizador não encontrado' };
      return;
    }

    this.isLoadingCustomerProfile = true;
    this.error = null;

    const request: CompleteCustomerProfileRequest = {
      userId: this.currentUser.id,
      firstName: formData.firstName,
      lastName: formData.lastName,
      phone: formData.phone,
      type: formData.type,
      street: formData.address.street,
      number: formData.address.number,
      parish: formData.address.parish,
      municipality: formData.address.municipality,
      district: formData.address.district,
      postalCode: formData.address.postalCode,
      complement: formData.address.complement,
      nif: formData.nif,
      citizenCard: formData.citizenCard,
    };

    this.customerService.completeProfile(request).subscribe({
      next: result => {
        this.isLoadingCustomerProfile = false;
        if (result.isSuccess && result.value) {
          this.successMessage = 'Perfil de cliente completado com sucesso!';
          this.isEditingCustomerProfile = false;
          
          // Refresh user data
          if (this.currentUser) {
            this.currentUser.customerId = result.value.customerId;
          }
          
          // Auto-hide success message
          setTimeout(() => (this.successMessage = null), 5000);
        } else {
          this.error = result.error || { code: 'UNKNOWN', message: 'Erro desconhecido' };
        }
      },
      error: err => {
        this.isLoadingCustomerProfile = false;
        this.error = {
          code: 'SUBMIT_ERROR',
          message: err?.error?.message || 'Erro ao completar perfil de cliente',
        };
      },
    });
  }

  // Handle customer profile form cancel
  onCustomerProfileCancel(): void {
    this.isEditingCustomerProfile = false;
  }

  // Scroll to customer form
  scrollToCustomerForm(): void {
    setTimeout(() => {
      const element = document.getElementById('customer-profile-section');
      if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    }, 100);
  }
}
