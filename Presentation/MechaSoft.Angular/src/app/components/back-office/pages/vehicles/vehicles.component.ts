import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { VehicleService } from '../../../../core/services/vehicle.service';
import { CustomerService } from '../../../../core/services/customer.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Vehicle, CreateVehicleRequest, Customer } from '../../../../core/models/api.models';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ToastService } from '../../../../core/services/toast.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-vehicles',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, ErrorMessageComponent],
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.scss']
})
export class VehiclesComponent implements OnInit {
  vehicles: Vehicle[] = [];
  customers: Customer[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  searchTerm: string = '';
  
  showModal: boolean = false;
  isEditMode: boolean = false;
  selectedVehicleId: string | null = null;
  
  vehicleForm: FormGroup;
  error: ErrorDetail | null = null;
  loading$;

  // Enum FuelType do backend (EN) com labels PT
  fuelTypes = [
    { value: 'Gasoline', label: 'Gasolina' },
    { value: 'Diesel', label: 'Diesel' },
    { value: 'Electric', label: 'Elétrico' },
    { value: 'Hybrid', label: 'Híbrido' },
    { value: 'LPG', label: 'GPL' },
    { value: 'CNG', label: 'GNC' }
  ];

  constructor(
    private vehicleService: VehicleService,
    private customerService: CustomerService,
    private authService: AuthService,
    private loadingService: LoadingService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
    private toastService: ToastService
  ) {
    this.vehicleForm = this.createForm();
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadVehicles();
    this.loadCustomers(); // Owner/Admin precisa ver clientes para criar veículos
  }

  // Criar formulário com validações
  private createForm(): FormGroup {
    return this.fb.group({
      customerId: ['', Validators.required],
      brand: ['', [Validators.required, Validators.minLength(2)]],
      model: ['', [Validators.required, Validators.minLength(2)]],
      licensePlate: ['', [Validators.required, Validators.pattern(/^[A-Z0-9-]{6,10}$/)]],
      color: ['', Validators.required],
      year: ['', [Validators.required, Validators.min(1900), Validators.max(new Date().getFullYear() + 1)]],
      vin: ['', [Validators.minLength(17), Validators.maxLength(17)]],
      engineType: [''],
      fuelType: ['', Validators.required]
    });
  }

  // Carregar veículos da API
  loadVehicles(): void {
    const currentUser = this.authService.getCurrentUser();
    const customerId = currentUser?.role === 'Customer' ? currentUser.customerId : undefined;

    this.vehicleService.getAll(this.currentPage, this.pageSize, customerId).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.vehicles = result.value.items;
        this.totalCount = result.value.totalCount;
        this.cdr.detectChanges();
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Carregar clientes para dropdown (Admin/Owner)
  loadCustomers(): void {
    const currentUser = this.authService.getCurrentUser();
    if (currentUser?.role === 'Owner' || currentUser?.role === 'Admin') {
      this.customerService.getAll({ pageNumber: 1, pageSize: 100 }).subscribe(result => {
        if (result.isSuccess && result.value) {
          this.customers = result.value.items;
        }
      });
    }
  }

  // Abrir modal para criar
  openCreateModal(): void {
    this.isEditMode = false;
    this.selectedVehicleId = null;
    this.vehicleForm.reset();
    this.showModal = true;
  }

  // Abrir modal para editar
  openEditModal(vehicle: Vehicle): void {
    this.isEditMode = true;
    this.selectedVehicleId = vehicle.id;
    this.vehicleForm.patchValue({
      customerId: vehicle.customerId,
      brand: vehicle.brand,
      model: vehicle.model,
      licensePlate: vehicle.licensePlate,
      color: vehicle.color,
      year: vehicle.year,
      vin: vehicle.vin,
      engineType: vehicle.engineType,
      fuelType: vehicle.fuelType
    });
    this.showModal = true;
  }

  // Fechar modal
  closeModal(): void {
    this.showModal = false;
    this.vehicleForm.reset();
    this.error = null;
  }

  // Submeter formulário
  onSubmit(): void {
    if (this.vehicleForm.invalid) {
      this.vehicleForm.markAllAsTouched();
      return;
    }

    const request: CreateVehicleRequest = this.vehicleForm.value;

    const operation$ = this.isEditMode && this.selectedVehicleId
      ? this.vehicleService.update(this.selectedVehicleId, request)
      : this.vehicleService.create(request);

    operation$.subscribe(result => {
      if (result.isSuccess) {
        if (this.isEditMode) {
          this.toastService.successUpdate('Veículo');
        } else {
          this.toastService.successCreate('Veículo');
        }
        this.closeModal();
        this.loadVehicles();
      } else {
        if (this.isEditMode) {
          this.toastService.errorUpdate('veículo');
        } else {
          this.toastService.errorCreate('veículo');
        }
        this.error = result.error || null;
      }
    });
  }

  // Pesquisar
  onSearch(term: string): void {
    this.searchTerm = term;
    this.currentPage = 1;
    this.loadVehicles();
  }

  // Paginação
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadVehicles();
  }

  // Obter número total de páginas
  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // Helper para validação
  isFieldInvalid(fieldName: string): boolean {
    const field = this.vehicleForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  // Obter erro do campo
  getFieldError(fieldName: string): string {
    const field = this.vehicleForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'Campo obrigatório';
    if (field.errors['minlength']) return `Mínimo ${field.errors['minlength'].requiredLength} caracteres`;
    if (field.errors['maxlength']) return `Máximo ${field.errors['maxlength'].requiredLength} caracteres`;
    if (field.errors['pattern']) return 'Formato inválido';
    if (field.errors['min']) return `Valor mínimo: ${field.errors['min'].min}`;
    if (field.errors['max']) return `Valor máximo: ${field.errors['max'].max}`;
    
    return 'Campo inválido';
  }

  // Obter nome do cliente
  getCustomerName(customerId: string): string {
    const customer = this.customers.find(c => c.id === customerId);
    return customer?.name || 'Desconhecido';
  }

  // Check if user can create vehicles (Admin/Owner)
  canManageVehicles(): boolean {
    const user = this.authService.getCurrentUser();
    return user?.role === 'Owner' || user?.role === 'Admin';
  }

  // Traduzir FuelType do enum para português
  getFuelTypeLabel(fuelType: string): string {
    const fuelMap: { [key: string]: string } = {
      'Gasoline': 'Gasolina',
      'Diesel': 'Diesel',
      'Electric': 'Elétrico',
      'Hybrid': 'Híbrido',
      'LPG': 'GPL',
      'CNG': 'GNC'
    };
    return fuelMap[fuelType] || fuelType;
  }
}
