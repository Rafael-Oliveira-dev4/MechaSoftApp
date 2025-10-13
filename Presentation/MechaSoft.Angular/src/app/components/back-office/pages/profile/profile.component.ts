import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { User } from '../../../../core/models/api.models';
import { ErrorDetail } from '../../../../core/models/result.model';
import { AuthService } from '../../../../core/services/auth.service';
import { CustomerService, CompleteCustomerProfileRequest } from '../../../../core/services/customer.service';
import { EmployeeService } from '../../../../core/services/employee.service';
import { Employee } from '../../../../core/models/employee.model';
import { usernameAvailabilityExcludeValidator } from '../../../../core/validators/username-availability-exclude.validator';
import { emailAvailabilityExcludeValidator } from '../../../../core/validators/email-availability-exclude.validator';
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
    ReactiveFormsModule,
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
  employeeData: Employee | null = null;
  isEditingProfile: boolean = false;
  isEditingCustomerProfile: boolean = false;
  isLoadingCustomerProfile: boolean = false;
  isLoadingEmployeeProfile: boolean = false;
  isLoadingProfileUpdate: boolean = false;
  error: ErrorDetail | null = null;
  successMessage: string | null = null;

  // Edit form with validation
  editForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private customerService: CustomerService,
    private employeeService: EmployeeService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      if (user?.username && user?.email) {
        this.initializeEditForm(user);
      }
      
      // Load employee data if user has employeeId
      if (user?.employeeId) {
        this.loadEmployeeData(user.employeeId);
      }
    });
  }

  // Load employee data
  private loadEmployeeData(employeeId: string): void {
    this.isLoadingEmployeeProfile = true;
    this.employeeService.getById(employeeId).subscribe(result => {
      this.isLoadingEmployeeProfile = false;
      if (result.isSuccess && result.value) {
        this.employeeData = result.value;
        this.cdr.detectChanges(); // Force change detection to render employee section
      }
    });
  }

  // Initialize edit form with validations
  private initializeEditForm(user: User): void {
    this.editForm = this.fb.group({
      username: [
        user.username,
        {
          validators: [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(50),
            Validators.pattern(/^[a-zA-Z0-9_]+$/),
          ],
          asyncValidators: [usernameAvailabilityExcludeValidator(this.authService, user.username)],
          updateOn: 'change',
        },
      ],
      email: [
        user.email,
        {
          validators: [Validators.required, Validators.email],
          asyncValidators: [emailAvailabilityExcludeValidator(this.authService, user.email)],
          updateOn: 'change',
        },
      ],
    });
  }

  // Toggle edit mode
  toggleEditMode(): void {
    this.isEditingProfile = !this.isEditingProfile;
    if (!this.isEditingProfile && this.currentUser) {
      // Reset form
      this.initializeEditForm(this.currentUser);
    }
  }

  // Save profile changes
  saveProfile(): void {
    // Bloquear submit se ainda está validando
    if (this.editForm.pending) {
      return;
    }

    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      return;
    }

    if (!this.currentUser?.id) {
      this.error = { code: 'NO_USER', message: 'Utilizador não encontrado' };
      return;
    }

    this.isLoadingProfileUpdate = true;
    this.error = null;

    const { username, email } = this.editForm.value;

    this.authService.updateUserProfile(this.currentUser.id, username, email).subscribe({
      next: result => {
        this.isLoadingProfileUpdate = false;
        if (result.isSuccess && result.value) {
          this.successMessage = '✅ Perfil atualizado com sucesso!';
          this.isEditingProfile = false;

          // Update current user
          if (this.currentUser) {
            this.currentUser.username = result.value.username;
            this.currentUser.email = result.value.email;
          }

          setTimeout(() => (this.successMessage = null), 5000);
        } else {
          this.error = result.error || { code: 'UNKNOWN', message: 'Erro desconhecido' };
        }
      },
      error: err => {
        this.isLoadingProfileUpdate = false;
        this.error = {
          code: 'UPDATE_ERROR',
          message: err?.message || 'Erro ao atualizar perfil',
        };
      },
    });
  }

  // Helper para validação de campos
  isFieldInvalid(fieldName: string): boolean {
    const field = this.editForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.editForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'Campo obrigatório';
    if (field.errors['minlength'])
      return `Mínimo ${field.errors['minlength'].requiredLength} caracteres`;
    if (field.errors['maxlength'])
      return `Máximo ${field.errors['maxlength'].requiredLength} caracteres`;
    if (field.errors['email']) return 'Email inválido';
    if (field.errors['emailTaken']) return 'Este email já está registado';
    if (field.errors['usernameTaken']) return 'Este nome de utilizador já está em uso';
    if (field.errors['pattern']) {
      if (fieldName === 'username') {
        return 'Apenas letras, números e underscore (_)';
      }
    }

    return 'Campo inválido';
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

  // Check if user is employee/owner
  isEmployeeUser(): boolean {
    return this.currentUser?.role === 'Employee' || this.currentUser?.role === 'Owner' || this.currentUser?.role === 'Admin';
  }

  // Check if user has employee profile
  hasEmployeeProfile(): boolean {
    return !!this.currentUser?.employeeId && !!this.employeeData;
  }

  // Get employee specialties as Portuguese labels
  getEmployeeSpecialties(): string {
    if (!this.employeeData?.specialties || this.employeeData.specialties.length === 0) return 'Nenhuma';
    
    const specialtyMap: { [key: string]: string } = {
      'Engine': 'Motor',
      'Transmission': 'Transmissão',
      'Brakes': 'Travões',
      'Suspension': 'Suspensão',
      'Electrical': 'Elétrico',
      'AirConditioning': 'Ar Condicionado',
      'Bodywork': 'Chapa/Pintura',
      'Maintenance': 'Manutenção',
      'Diagnostic': 'Diagnóstico',
      'Inspection': 'Inspeção',
      'Tires': 'Pneus',
      'Exhaust': 'Escape',
      'Cooling': 'Arrefecimento',
      'Fuel': 'Combustível'
    };
    
    return this.employeeData.specialties
      .map(s => specialtyMap[s] || s)
      .join(', ');
  }

  // Get employee role label
  getEmployeeRoleLabel(): string {
    const roleMap: { [key: string]: string } = {
      'Owner': 'Proprietário',
      'Mechanic': 'Mecânico',
      'Manager': 'Gerente',
      'Receptionist': 'Rececionista',
      'PartsClerk': 'Responsável de Peças'
    };
    return roleMap[this.employeeData?.role || ''] || this.employeeData?.role || '';
  }

  // Format currency
  formatCurrency(value?: number): string {
    if (!value) return '-';
    return new Intl.NumberFormat('pt-PT', {
      style: 'currency',
      currency: 'EUR'
    }).format(value);
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
          this.successMessage = `✅ Perfil completado com sucesso! Bem-vindo(a), ${result.value.fullName}!`;
          this.isEditingCustomerProfile = false;
          
          // Refresh user data
          if (this.currentUser) {
            this.currentUser.customerId = result.value.customerId;
          }
          
          // Auto-hide success message after 6 seconds
          setTimeout(() => (this.successMessage = null), 6000);
        } else {
          this.error = result.error || { code: 'UNKNOWN', message: 'Erro desconhecido' };
        }
      },
      error: err => {
        this.isLoadingCustomerProfile = false;
        this.error = {
          code: 'SUBMIT_ERROR',
          message: err?.error?.message || err?.message || 'Erro ao completar perfil de cliente',
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
