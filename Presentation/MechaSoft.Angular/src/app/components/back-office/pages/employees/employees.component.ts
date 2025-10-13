import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { EmployeeService } from '../../../../core/services/employee.service';
import { Employee, CreateEmployeeRequest, UpdateEmployeeRequest } from '../../../../core/models/employee.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ToastService } from '../../../../core/services/toast.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, ErrorMessageComponent],
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})
export class EmployeesComponent implements OnInit {
  employees: Employee[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  
  showModal: boolean = false;
  isEditMode: boolean = false;
  selectedEmployeeId: string | null = null;
  
  // Credentials modal for newly created employee
  showCredentialsModal: boolean = false;
  generatedCredentials: { username: string; password: string; employeeName: string } | null = null;
  
  employeeForm: FormGroup;
  error: ErrorDetail | null = null;
  loading$;

  // Enum EmployeeRole do backend
  roles = [
    { value: 'Owner', label: 'Proprietário' },
    { value: 'Mechanic', label: 'Mecânico' },
    { value: 'Manager', label: 'Gerente' },
    { value: 'Receptionist', label: 'Rececionista' },
    { value: 'PartsClerk', label: 'Responsável de Peças' }
  ];

  // Enum ServiceCategory do backend
  specialtiesOptions = [
    { value: 'Engine', label: 'Motor' },
    { value: 'Transmission', label: 'Transmissão' },
    { value: 'Brakes', label: 'Travões' },
    { value: 'Suspension', label: 'Suspensão' },
    { value: 'Electrical', label: 'Elétrico' },
    { value: 'AirConditioning', label: 'Ar Condicionado' },
    { value: 'Bodywork', label: 'Chapa/Pintura' },
    { value: 'Maintenance', label: 'Manutenção' },
    { value: 'Diagnostic', label: 'Diagnóstico' },
    { value: 'Inspection', label: 'Inspeção' },
    { value: 'Tires', label: 'Pneus' },
    { value: 'Exhaust', label: 'Escape' },
    { value: 'Cooling', label: 'Arrefecimento' },
    { value: 'Fuel', label: 'Combustível' }
  ];

  constructor(
    private employeeService: EmployeeService,
    private loadingService: LoadingService,
    private toastService: ToastService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.employeeForm = this.createForm();
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  private createForm(): FormGroup {
    return this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^\+?[0-9\s\-()]{9,}$/)]],
      role: ['Mechanic', Validators.required],
      hourlyRate: [0, [Validators.required, Validators.min(0)]],
      specialties: [[]],
      canPerformInspections: [false],
      inspectionLicenseNumber: ['']
    });
  }

  loadEmployees(): void {
    this.employeeService.getAll(this.currentPage, this.pageSize).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.employees = result.value.items;
        this.totalCount = result.value.totalCount;
        this.cdr.detectChanges();
      } else {
        this.error = result.error || null;
      }
    });
  }

  openCreateModal(): void {
    this.isEditMode = false;
    this.selectedEmployeeId = null;
    this.employeeForm.reset({
      role: 'Mechanic',
      hourlyRate: 0,
      specialties: [],
      canPerformInspections: false
    });
    this.showModal = true;
  }

  openEditModal(employee: Employee): void {
    this.isEditMode = true;
    this.selectedEmployeeId = employee.id;
    this.employeeForm.patchValue({
      firstName: employee.firstName,
      lastName: employee.lastName,
      email: employee.email,
      phone: employee.phone,
      role: employee.role,
      hourlyRate: employee.hourlyRate || 0,
      specialties: employee.specialties || [],
      canPerformInspections: employee.canPerformInspections,
      inspectionLicenseNumber: employee.inspectionLicenseNumber || ''
    });
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.employeeForm.reset();
    this.error = null;
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    const formValue = this.employeeForm.value;
    const request = {
      ...formValue,
      hourlyRate: formValue.hourlyRate || null
    };

    const operation$ = this.isEditMode && this.selectedEmployeeId
      ? this.employeeService.update(this.selectedEmployeeId, request as UpdateEmployeeRequest)
      : this.employeeService.create(request as CreateEmployeeRequest);

    operation$.subscribe(result => {
      if (result.isSuccess) {
        // Show success toast
        if (this.isEditMode) {
          this.toastService.successUpdate('Funcionário');
        } else {
          this.toastService.successCreate('Funcionário');
        }

        this.closeModal();
        this.loadEmployees();

        // Check if backend auto-generated user credentials (only for CREATE)
        if (!this.isEditMode && result.value) {
          const response = result.value as any;
          console.log('🔍 CREATE EMPLOYEE RESPONSE:', response);
          console.log('   generatedUsername:', response.generatedUsername);
          console.log('   generatedPassword:', response.generatedPassword);
          
          if (response.generatedUsername && response.generatedPassword) {
            // Show credentials modal
            this.generatedCredentials = {
              username: response.generatedUsername,
              password: response.generatedPassword,
              employeeName: response.fullName || formValue.firstName + ' ' + formValue.lastName
            };
            this.showCredentialsModal = true;
            console.log('✅ Credentials modal opened!');
          } else {
            console.warn('⚠️ No credentials in response. Backend may not be returning them.');
          }
        }
      } else {
        // Show error toast
        if (this.isEditMode) {
          this.toastService.errorUpdate('funcionário');
        } else {
          this.toastService.errorCreate('funcionário');
        }
        this.error = result.error || null;
      }
    });
  }

  // Close credentials modal
  closeCredentialsModal(): void {
    this.showCredentialsModal = false;
    this.generatedCredentials = null;
  }

  // Copy credentials to clipboard
  copyToClipboard(text: string): void {
    navigator.clipboard.writeText(text).then(() => {
      this.toastService.info('Copiado para a área de transferência!', 'Copiado!');
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadEmployees();
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  getRoleLabel(role: string): string {
    const roleObj = this.roles.find(r => r.value === role);
    return roleObj?.label || role;
  }

  getSpecialtiesLabel(specialties: string[] | undefined): string {
    if (!specialties || specialties.length === 0) return 'Nenhuma';
    const labels = specialties
      .map(s => this.specialtiesOptions.find(opt => opt.value === s)?.label || s)
      .slice(0, 3); // Mostrar apenas primeiras 3
    
    const remaining = specialties.length - labels.length;
    return labels.join(', ') + (remaining > 0 ? ` +${remaining}` : '');
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-PT', {
      style: 'currency',
      currency: 'EUR'
    }).format(value);
  }

  // Helpers de validação
  isFieldInvalid(fieldName: string): boolean {
    const field = this.employeeForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.employeeForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'Campo obrigatório';
    if (field.errors['minlength']) return `Mínimo ${field.errors['minlength'].requiredLength} caracteres`;
    if (field.errors['email']) return 'Email inválido';
    if (field.errors['pattern']) return 'Formato inválido';
    if (field.errors['min']) return `Valor mínimo: ${field.errors['min'].min}`;
    
    return 'Campo inválido';
  }

  // Toggle specialty selection
  toggleSpecialty(specialty: string): void {
    const specialties = this.employeeForm.get('specialties')?.value || [];
    const index = specialties.indexOf(specialty);
    
    if (index > -1) {
      specialties.splice(index, 1);
    } else {
      specialties.push(specialty);
    }
    
    this.employeeForm.patchValue({ specialties });
  }

  isSpecialtySelected(specialty: string): boolean {
    const specialties = this.employeeForm.get('specialties')?.value || [];
    return specialties.includes(specialty);
  }
}

