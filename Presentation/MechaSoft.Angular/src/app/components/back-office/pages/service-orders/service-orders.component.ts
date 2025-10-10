import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ServiceOrderService } from '../../../../core/services/service-order.service';
import { CustomerService } from '../../../../core/services/customer.service';
import { VehicleService } from '../../../../core/services/vehicle.service';
import { EmployeeService } from '../../../../core/services/employee.service';
import { ServiceOrder as BaseServiceOrder, Customer, Vehicle } from '../../../../core/models/api.models';
import { Employee } from '../../../../core/models/employee.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

// Extended interface for client view
interface ServiceOrder extends BaseServiceOrder {
  customerName?: string;
  vehiclePlate?: string;
  vehicleInfo?: string;
  mechanicName?: string | null;
}

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
    { value: 'InProgress', label: 'Em Curso', color: 'blue' },
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
    // this.loadCustomers(); // Comentado - cliente não precisa ver outros clientes
    // this.loadMechanics(); // Comentado - cliente não precisa ver mecânicos
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

  // Carregar ordens (Mock data para cliente)
  loadOrders(): void {
    // Mock data - substituir com API real quando disponível
    this.orders = [
      {
        id: '1',
        orderNumber: 'OS-2024-156',
        customerId: 'current-user',
        customerName: 'rafael_oliveira',
        vehicleId: '1',
        vehiclePlate: '12-AB-34',
        vehicleInfo: 'BMW 320d',
        description: 'Troca de óleo e filtros + revisão geral dos 10.000 km',
        priority: 'Normal',
        status: 'InProgress',
        estimatedCost: 320.50,
        finalCost: 0,
        estimatedDelivery: new Date('2024-10-12T17:00:00'),
        actualDelivery: undefined,
        requiresInspection: true,
        mechanicId: 'mech-1',
        mechanicName: 'João Silva',
        createdAt: new Date('2024-10-09T09:00:00'),
        updatedAt: new Date('2024-10-10T14:30:00'),
        internalNotes: ''
      },
      {
        id: '2',
        orderNumber: 'OS-2024-142',
        customerId: 'current-user',
        customerName: 'rafael_oliveira',
        vehicleId: '2',
        vehiclePlate: '56-CD-78',
        vehicleInfo: 'Volkswagen Golf 1.6 TDI',
        description: 'Revisão periódica - Inspeção completa e troca de pastilhas de travão',
        priority: 'Normal',
        status: 'Pending',
        estimatedCost: 450.00,
        finalCost: 0,
        estimatedDelivery: new Date('2024-10-15T16:00:00'),
        actualDelivery: undefined,
        requiresInspection: true,
        mechanicId: undefined,
        mechanicName: undefined,
        createdAt: new Date('2024-09-28T11:00:00'),
        updatedAt: new Date('2024-09-28T11:00:00'),
        internalNotes: ''
      },
      {
        id: '3',
        orderNumber: 'OS-2024-098',
        customerId: 'current-user',
        customerName: 'rafael_oliveira',
        vehicleId: '1',
        vehiclePlate: '12-AB-34',
        vehicleInfo: 'BMW 320d',
        description: 'Substituição de pastilhas de travão dianteiras e discos',
        priority: 'High',
        status: 'Completed',
        estimatedCost: 580.00,
        finalCost: 654.50,
        estimatedDelivery: new Date('2024-09-07T18:00:00'),
        actualDelivery: new Date('2024-09-05T16:30:00'),
        requiresInspection: false,
        mechanicId: 'mech-2',
        mechanicName: 'Pedro Costa',
        createdAt: new Date('2024-09-03T10:00:00'),
        updatedAt: new Date('2024-09-05T16:30:00'),
        internalNotes: ''
      },
      {
        id: '4',
        orderNumber: 'OS-2024-067',
        customerId: 'current-user',
        customerName: 'rafael_oliveira',
        vehicleId: '2',
        vehiclePlate: '56-CD-78',
        vehicleInfo: 'Volkswagen Golf 1.6 TDI',
        description: 'Alinhamento e balanceamento das rodas',
        priority: 'Low',
        status: 'Completed',
        estimatedCost: 80.00,
        finalCost: 75.00,
        estimatedDelivery: new Date('2024-08-22T12:00:00'),
        actualDelivery: new Date('2024-08-20T11:00:00'),
        requiresInspection: false,
        mechanicId: 'mech-1',
        mechanicName: 'João Silva',
        createdAt: new Date('2024-08-19T09:00:00'),
        updatedAt: new Date('2024-08-20T11:00:00'),
        internalNotes: ''
      },
      {
        id: '5',
        orderNumber: 'OS-2024-023',
        customerId: 'current-user',
        customerName: 'rafael_oliveira',
        vehicleId: '1',
        vehiclePlate: '12-AB-34',
        vehicleInfo: 'BMW 320d',
        description: 'Diagnóstico eletrónico - Problema com sensor de oxigénio',
        priority: 'Urgent',
        status: 'Completed',
        estimatedCost: 150.00,
        finalCost: 180.00,
        estimatedDelivery: new Date('2024-07-17T14:00:00'),
        actualDelivery: new Date('2024-07-15T15:00:00'),
        requiresInspection: true,
        mechanicId: 'mech-3',
        mechanicName: 'Carlos Mendes',
        createdAt: new Date('2024-07-14T16:00:00'),
        updatedAt: new Date('2024-07-15T15:00:00'),
        internalNotes: ''
      }
    ];

    // Filtrar por status se necessário
    if (this.statusFilter) {
      this.orders = this.orders.filter(o => o.status === this.statusFilter);
    }

    this.totalCount = this.orders.length;
    
    /* Código real API - descomentar quando backend estiver pronto
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
    */
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
