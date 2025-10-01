import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ServiceOrderService } from '../../../../core/services/service-order.service';
import { CustomerService } from '../../../../core/services/customer.service';
import { VehicleService } from '../../../../core/services/vehicle.service';
import { EmployeeService } from '../../../../core/services/employee.service';
import { ServiceOrder, Customer, Vehicle } from '../../../../core/models/api.models';
import { Employee } from '../../../../core/models/employee.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-service-orders',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, ErrorMessageComponent],
  templateUrl: './service-orders.component.html',
  styleUrls: ['./service-orders.component.scss']
})
export class ServiceOrdersComponent implements OnInit {
  orders: ServiceOrder[] = [];
  customers: Customer[] = [];
  vehicles: Vehicle[] = [];
  mechanics: Employee[] = [];
  
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  statusFilter: string = '';
  
  showCreateModal: boolean = false;
  showDetailsModal: boolean = false;
  selectedOrder: ServiceOrder | null = null;
  
  orderForm: FormGroup;
  error: ErrorDetail | null = null;
  loading$;

  statuses = [
    { value: 'Pending', label: 'Pendente', color: 'yellow' },
    { value: 'InProgress', label: 'Em Progresso', color: 'blue' },
    { value: 'Completed', label: 'Concluída', color: 'green' },
    { value: 'Cancelled', label: 'Cancelada', color: 'red' }
  ];

  priorities = [
    { value: 'Low', label: 'Baixa' },
    { value: 'Normal', label: 'Normal' },
    { value: 'High', label: 'Alta' },
    { value: 'Urgent', label: 'Urgente' }
  ];

  constructor(
    private serviceOrderService: ServiceOrderService,
    private customerService: CustomerService,
    private vehicleService: VehicleService,
    private employeeService: EmployeeService,
    private loadingService: LoadingService,
    private fb: FormBuilder
  ) {
    this.orderForm = this.createForm();
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadOrders();
    this.loadCustomers();
    this.loadMechanics();
  }

  // Criar formulário
  private createForm(): FormGroup {
    return this.fb.group({
      customerId: ['', Validators.required],
      vehicleId: ['', Validators.required],
      description: ['', [Validators.required, Validators.minLength(10)]],
      priority: ['Normal', Validators.required],
      estimatedCost: [0, [Validators.required, Validators.min(0)]],
      estimatedDelivery: [''],
      requiresInspection: [false],
      internalNotes: ['']
    });
  }

  // Carregar ordens
  loadOrders(): void {
    this.serviceOrderService.getAll(
      this.currentPage,
      this.pageSize,
      this.statusFilter || undefined
    ).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.orders = result.value.items || [];
        this.totalCount = result.value.totalCount || 0;
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Carregar clientes
  loadCustomers(): void {
    this.customerService.getAll({ pageSize: 100 }).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.customers = result.value.items;
      }
    });
  }

  // Carregar mecânicos
  loadMechanics(): void {
    this.employeeService.getMechanics().subscribe(result => {
      if (result.isSuccess && result.value) {
        this.mechanics = result.value;
      }
    });
  }

  // Carregar veículos do cliente selecionado
  onCustomerChange(customerId: string): void {
    if (customerId) {
      this.vehicleService.getByCustomer(customerId).subscribe(result => {
        if (result.isSuccess && result.value) {
          this.vehicles = result.value;
        }
      });
    } else {
      this.vehicles = [];
      this.orderForm.patchValue({ vehicleId: '' });
    }
  }

  // Abrir modal para criar
  openCreateModal(): void {
    this.orderForm.reset({ priority: 'Normal', requiresInspection: false });
    this.vehicles = [];
    this.showCreateModal = true;
  }

  // Fechar modal de criação
  closeCreateModal(): void {
    this.showCreateModal = false;
    this.orderForm.reset();
    this.error = null;
  }

  // Submeter formulário
  onSubmit(): void {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched();
      return;
    }

    const request = this.orderForm.value;
    
    this.serviceOrderService.create(request).subscribe(result => {
      if (result.isSuccess) {
        this.closeCreateModal();
        this.loadOrders();
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Ver detalhes da ordem
  viewDetails(order: ServiceOrder): void {
    this.selectedOrder = order;
    this.showDetailsModal = true;
  }

  // Fechar modal de detalhes
  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedOrder = null;
  }

  // Atualizar status
  updateStatus(order: ServiceOrder, newStatus: string): void {
    if (confirm(`Alterar estado para "${this.getStatusLabel(newStatus)}"?`)) {
      this.serviceOrderService.updateStatus(order.id, newStatus).subscribe(result => {
        if (result.isSuccess) {
          this.loadOrders();
          this.closeDetailsModal();
        } else {
          this.error = result.error || null;
        }
      });
    }
  }

  // Atribuir mecânico
  assignMechanic(order: ServiceOrder, mechanicId: string): void {
    if (!mechanicId) return;
    
    this.serviceOrderService.assignMechanic(order.id, mechanicId).subscribe(result => {
      if (result.isSuccess) {
        this.loadOrders();
        this.closeDetailsModal();
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Cancelar ordem
  cancelOrder(order: ServiceOrder): void {
    const reason = prompt('Motivo do cancelamento:');
    if (reason) {
      this.serviceOrderService.cancel(order.id, reason).subscribe(result => {
        if (result.isSuccess) {
          this.loadOrders();
          this.closeDetailsModal();
        } else {
          this.error = result.error || null;
        }
      });
    }
  }

  // Filtrar por status
  filterByStatus(status: string): void {
    this.statusFilter = status;
    this.currentPage = 1;
    this.loadOrders();
  }

  // Paginação
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadOrders();
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // Helpers
  getStatusLabel(status: string): string {
    return this.statuses.find(s => s.value === status)?.label || status;
  }

  getStatusColor(status: string): string {
    return this.statuses.find(s => s.value === status)?.color || 'gray';
  }

  getPriorityLabel(priority: string): string {
    return this.priorities.find(p => p.value === priority)?.label || priority;
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.orderForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.orderForm.get(fieldName);
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
