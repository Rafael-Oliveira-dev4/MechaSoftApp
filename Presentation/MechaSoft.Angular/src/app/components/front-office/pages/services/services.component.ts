import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MechanicServiceService } from '../../../../core/services/mechanic-service.service';
import { MechanicService, CreateServiceRequest, UpdateServiceRequest } from '../../../../core/models/service.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, ErrorMessageComponent],
  templateUrl: './services.component.html',
  styleUrls: ['./services.component.scss']
})
export class ServicesComponent implements OnInit {
  services: MechanicService[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  
  showModal: boolean = false;
  isEditMode: boolean = false;
  selectedServiceId: string | null = null;
  
  serviceForm: FormGroup;
  error: ErrorDetail | null = null;
  loading$;

  categories = ['Manutenção', 'Reparação', 'Revisão', 'Diagnóstico', 'Inspeção', 'Pintura', 'Chapa'];

  constructor(
    private mechanicServiceService: MechanicServiceService,
    private loadingService: LoadingService,
    private fb: FormBuilder
  ) {
    this.serviceForm = this.createForm();
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadServices();
  }

  // Criar formulário
  private createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      category: ['', Validators.required],
      estimatedHours: [0, [Validators.required, Validators.min(0)]],
      pricePerHour: [0],
      fixedPrice: [0],
      requiresInspection: [false]
    });
  }

  // Carregar serviços
  loadServices(): void {
    this.mechanicServiceService.getAll(this.currentPage, this.pageSize).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.services = result.value.items;
        this.totalCount = result.value.totalCount;
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Abrir modal para criar
  openCreateModal(): void {
    this.isEditMode = false;
    this.selectedServiceId = null;
    this.serviceForm.reset({ requiresInspection: false });
    this.showModal = true;
  }

  // Abrir modal para editar
  openEditModal(service: MechanicService): void {
    this.isEditMode = true;
    this.selectedServiceId = service.id;
    this.serviceForm.patchValue({
      name: service.name,
      description: service.description,
      category: service.category,
      estimatedHours: service.estimatedHours,
      pricePerHour: service.pricePerHour,
      fixedPrice: service.fixedPrice,
      requiresInspection: service.requiresInspection
    });
    this.showModal = true;
  }

  // Fechar modal
  closeModal(): void {
    this.showModal = false;
    this.serviceForm.reset();
    this.error = null;
  }

  // Submeter formulário
  onSubmit(): void {
    if (this.serviceForm.invalid) {
      this.serviceForm.markAllAsTouched();
      return;
    }

    const request = this.serviceForm.value;

    const operation$ = this.isEditMode && this.selectedServiceId
      ? this.mechanicServiceService.update(this.selectedServiceId, request as UpdateServiceRequest)
      : this.mechanicServiceService.create(request as CreateServiceRequest);

    operation$.subscribe(result => {
      if (result.isSuccess) {
        this.closeModal();
        this.loadServices();
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Paginação
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadServices();
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // Calcular preço estimado
  calculateEstimatedPrice(service: MechanicService): number {
    if (service.fixedPrice && service.fixedPrice > 0) {
      return service.fixedPrice;
    }
    if (service.pricePerHour && service.estimatedHours > 0) {
      return service.pricePerHour * service.estimatedHours;
    }
    return 0;
  }

  // Helpers de validação
  isFieldInvalid(fieldName: string): boolean {
    const field = this.serviceForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.serviceForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'Campo obrigatório';
    if (field.errors['minlength']) return `Mínimo ${field.errors['minlength'].requiredLength} caracteres`;
    if (field.errors['min']) return `Valor mínimo: ${field.errors['min'].min}`;
    
    return 'Campo inválido';
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-PT', {
      style: 'currency',
      currency: 'EUR'
    }).format(value);
  }
}
