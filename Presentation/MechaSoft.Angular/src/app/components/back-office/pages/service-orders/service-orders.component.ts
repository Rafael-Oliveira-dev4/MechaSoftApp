import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ServiceOrderService, AddServiceToOrderRequest, AddPartToOrderRequest } from '../../../../core/services/service-order.service';
import { CustomerService } from '../../../../core/services/customer.service';
import { VehicleService } from '../../../../core/services/vehicle.service';
import { EmployeeService } from '../../../../core/services/employee.service';
import { AuthService } from '../../../../core/services/auth.service';
import { MechanicServiceService } from '../../../../core/services/mechanic-service.service';
import { PartService } from '../../../../core/services/part.service';
import { ServiceOrder as BaseServiceOrder, Customer, Vehicle } from '../../../../core/models/api.models';
import { Employee } from '../../../../core/models/employee.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ToastService } from '../../../../core/services/toast.service';
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
  availableServices: any[] = [];
  availableParts: any[] = [];
  
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  statusFilter: string = '';
  
  showCreateModal: boolean = false;
  showDetailsModal: boolean = false;
  showAddServiceModal: boolean = false;
  showAddPartModal: boolean = false;
  selectedOrder: ServiceOrder | null = null;
  
  orderForm: FormGroup;
  addServiceForm: FormGroup;
  addPartForm: FormGroup;
  error: ErrorDetail | null = null;
  loading$;

  statuses = [
    { value: 'Pending', label: 'Pendente', color: 'yellow' },
    { value: 'InProgress', label: 'Em Curso', color: 'blue' },
    { value: 'Completed', label: 'Concluída', color: 'green' },
    { value: 'Cancelled', label: 'Cancelada', color: 'red' }
  ];

  // Priority enum aligned with backend
  priorities = [
    { value: 'Low', label: 'Baixa' },
    { value: 'Medium', label: 'Média' },
    { value: 'High', label: 'Alta' },
    { value: 'Urgent', label: 'Urgente' }
  ];

  constructor(
    private serviceOrderService: ServiceOrderService,
    private customerService: CustomerService,
    private vehicleService: VehicleService,
    private employeeService: EmployeeService,
    private mechanicServiceService: MechanicServiceService,
    private partService: PartService,
    private authService: AuthService,
    private loadingService: LoadingService,
    private toastService: ToastService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.orderForm = this.createForm();
    this.addServiceForm = this.createAddServiceForm();
    this.addPartForm = this.createAddPartForm();
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadOrders();
    this.loadCustomers(); // Admin/Owner precisa ver clientes
    this.loadMechanics(); // Admin/Owner precisa atribuir mecânicos
  }

  // Criar formulário
  private createForm(): FormGroup {
    return this.fb.group({
      customerId: ['', Validators.required],
      vehicleId: ['', Validators.required],
      description: ['', [Validators.required, Validators.minLength(10)]],
      priority: ['Medium', Validators.required],
      estimatedCost: [0, [Validators.required, Validators.min(0)]],
      estimatedDelivery: [''],
      mechanicId: [''],
      requiresInspection: [false],
      internalNotes: ['']
    });
  }

  private createAddServiceForm(): FormGroup {
    return this.fb.group({
      serviceId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      estimatedHours: [1, [Validators.required, Validators.min(0.1)]],
      discountPercentage: [0, [Validators.min(0), Validators.max(100)]],
      mechanicId: ['']
    });
  }

  private createAddPartForm(): FormGroup {
    return this.fb.group({
      partId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      discountPercentage: [0, [Validators.min(0), Validators.max(100)]]
    });
  }

  // Carregar ordens da API
  loadOrders(): void {
    const currentUser = this.authService.getCurrentUser();
    const customerId = currentUser?.role === 'Customer' ? currentUser.customerId : undefined;

    this.serviceOrderService.getAll(this.currentPage, this.pageSize, this.statusFilter, customerId).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.orders = result.value.items;
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

  // Carregar mecânicos para dropdown (Admin/Owner)
  loadMechanics(): void {
    const currentUser = this.authService.getCurrentUser();
    if (currentUser?.role === 'Owner' || currentUser?.role === 'Admin') {
      this.employeeService.getAll(1, 100).subscribe(result => {
        if (result.isSuccess && result.value) {
          // Filter only mechanics/active employees
          this.mechanics = result.value.items.filter(e => e.isActive);
        }
      });
    }
  }

  // Carregar serviços disponíveis
  loadServices(): void {
    this.mechanicServiceService.getAll(1, 100).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.availableServices = result.value.items.filter((s: any) => s.isActive);
      }
    });
  }

  // Carregar peças disponíveis
  loadParts(): void {
    this.partService.getAll(1, 100).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.availableParts = result.value.items.filter((p: any) => p.isActive && p.stockQuantity > 0);
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

  // Helper para validação
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

  // Check if user can manage service orders (Admin/Owner)
  canManageOrders(): boolean {
    const user = this.authService.getCurrentUser();
    return user?.role === 'Owner' || user?.role === 'Admin';
  }

  // Traduzir Status do enum para português
  getStatusLabel(status: string): string {
    const statusMap: { [key: string]: string } = {
      'Pending': 'Pendente',
      'InProgress': 'Em Curso',
      'WaitingParts': 'Aguarda Peças',
      'WaitingApproval': 'Aguarda Aprovação',
      'WaitingInspection': 'Aguarda Inspeção',
      'Completed': 'Concluída',
      'Delivered': 'Entregue',
      'Cancelled': 'Cancelada'
    };
    return statusMap[status] || status;
  }

  // Get status color for badges (from statuses array)
  getStatusColor(status: string): string {
    return this.statuses.find(s => s.value === status)?.color || 'gray';
  }

  getServiceStatusColor(status: string): string {
    const colorMap: { [key: string]: string } = {
      'Pending': 'gray',
      'InProgress': 'blue',
      'Paused': 'yellow',
      'Completed': 'green',
      'Cancelled': 'red'
    };
    return colorMap[status] || 'gray';
  }

  // Traduzir Priority do enum para português
  getPriorityLabel(priority: string): string {
    const priorityMap: { [key: string]: string } = {
      'Low': 'Baixa',
      'Medium': 'Média',
      'High': 'Alta',
      'Urgent': 'Urgente'
    };
    return priorityMap[priority] || priority;
  }

  // Get priority color class
  getPriorityColorClass(priority: string): string {
    const colorMap: { [key: string]: string } = {
      'Low': 'badge-gray',
      'Medium': 'badge-blue',
      'High': 'badge-orange',
      'Urgent': 'badge-red'
    };
    return colorMap[priority] || 'badge-gray';
  }

  // Get status color class
  getStatusColorClass(status: string): string {
    const colorMap: { [key: string]: string } = {
      'Pending': 'badge-yellow',
      'InProgress': 'badge-blue',
      'WaitingParts': 'badge-purple',
      'WaitingApproval': 'badge-orange',
      'WaitingInspection': 'badge-teal',
      'Completed': 'badge-green',
      'Delivered': 'badge-green',
      'Cancelled': 'badge-red'
    };
    return colorMap[status] || 'badge-gray';
  }

  // ========== MODAIS DE ADICIONAR SERVIÇOS/PEÇAS ==========

  // Abrir modal para adicionar serviço
  openAddServiceModal(order: ServiceOrder): void {
    this.selectedOrder = order;
    this.loadServices();
    this.loadMechanics();
    this.addServiceForm.reset({
      serviceId: '',
      quantity: 1,
      estimatedHours: 1,
      discountPercentage: 0,
      mechanicId: ''
    });
    this.showAddServiceModal = true;
  }

  // Fechar modal de adicionar serviço
  closeAddServiceModal(): void {
    this.showAddServiceModal = false;
    this.addServiceForm.reset();
    this.selectedOrder = null;
  }

  // Submeter formulário de adicionar serviço
  submitAddService(): void {
    if (this.addServiceForm.invalid || !this.selectedOrder) {
      this.addServiceForm.markAllAsTouched();
      return;
    }

    const formValue = this.addServiceForm.value;
    const request: AddServiceToOrderRequest = {
      serviceId: formValue.serviceId,
      quantity: formValue.quantity,
      estimatedHours: formValue.estimatedHours,
      discountPercentage: formValue.discountPercentage || undefined,
      mechanicId: formValue.mechanicId || undefined
    };

    this.serviceOrderService.addService(this.selectedOrder.id, request).subscribe(result => {
      if (result.isSuccess) {
        this.toastService.success('Serviço adicionado à ordem com sucesso!');
        this.closeAddServiceModal();
        this.loadOrders(); // Refresh list
        // Optionally reload order details if modal is open
        if (this.showDetailsModal && this.selectedOrder) {
          this.viewDetails(this.selectedOrder);
        }
      } else {
        this.toastService.error('Erro ao adicionar serviço à ordem.');
        this.error = result.error || null;
      }
    });
  }

  // Abrir modal para adicionar peça
  openAddPartModal(order: ServiceOrder): void {
    this.selectedOrder = order;
    this.loadParts();
    this.addPartForm.reset({
      partId: '',
      quantity: 1,
      discountPercentage: 0
    });
    this.showAddPartModal = true;
  }

  // Fechar modal de adicionar peça
  closeAddPartModal(): void {
    this.showAddPartModal = false;
    this.addPartForm.reset();
    this.selectedOrder = null;
  }

  // Submeter formulário de adicionar peça
  submitAddPart(): void {
    if (this.addPartForm.invalid || !this.selectedOrder) {
      this.addPartForm.markAllAsTouched();
      return;
    }

    const formValue = this.addPartForm.value;
    const request: AddPartToOrderRequest = {
      partId: formValue.partId,
      quantity: formValue.quantity,
      discountPercentage: formValue.discountPercentage || undefined
    };

    this.serviceOrderService.addPart(this.selectedOrder.id, request).subscribe(result => {
      if (result.isSuccess) {
        this.toastService.success('Peça adicionada à ordem com sucesso!');
        this.closeAddPartModal();
        this.loadOrders(); // Refresh list
        // Optionally reload order details if modal is open
        if (this.showDetailsModal && this.selectedOrder) {
          this.viewDetails(this.selectedOrder);
        }
      } else {
        this.toastService.error('Erro ao adicionar peça à ordem.');
        this.error = result.error || null;
      }
    });
  }

  // ========== FIM MODAIS SERVIÇOS/PEÇAS ==========

  // ========== STATUS MANAGEMENT ==========

  // Mudar status da ordem
  changeOrderStatus(order: ServiceOrder, newStatus: string): void {
    if (!order || !newStatus) return;

    this.serviceOrderService.updateStatus(order.id, newStatus).subscribe(result => {
      if (result.isSuccess) {
        this.toastService.success('Estado da ordem atualizado!');
        this.loadOrders(); // Refresh list
        // Update selectedOrder if details modal is open
        if (this.showDetailsModal && this.selectedOrder?.id === order.id) {
          this.selectedOrder.status = newStatus;
          this.cdr.detectChanges();
        }
      } else {
        this.toastService.error('Erro ao atualizar estado da ordem.');
        this.error = result.error || null;
      }
    });
  }

  // Atribuir mecânico à ordem
  assignMechanicToOrder(order: ServiceOrder, mechanicId: string): void {
    if (!order || !mechanicId) return;

    this.serviceOrderService.assignMechanic(order.id, mechanicId).subscribe(result => {
      if (result.isSuccess) {
        this.toastService.success('Mecânico atribuído com sucesso!');
        this.loadOrders(); // Refresh list
        // Update selectedOrder if details modal is open
        if (this.showDetailsModal && this.selectedOrder?.id === order.id) {
          const mechanic = this.mechanics.find(m => m.id === mechanicId);
          if (mechanic) {
            this.selectedOrder.mechanicName = mechanic.fullName;
          }
          this.cdr.detectChanges();
        }
      } else {
        this.toastService.error('Erro ao atribuir mecânico.');
        this.error = result.error || null;
      }
    });
  }

  // ========== FIM STATUS MANAGEMENT ==========

  // Submeter formulário para criar/editar ordem
  onSubmit(): void {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched();
      return;
    }

    const formValue = this.orderForm.value;
    const request = {
      customerId: formValue.customerId,
      vehicleId: formValue.vehicleId,
      description: formValue.description,
      priority: formValue.priority,
      estimatedCost: formValue.estimatedCost,
      estimatedDelivery: formValue.estimatedDelivery || null,
      mechanicId: formValue.mechanicId || null,
      requiresInspection: formValue.requiresInspection,
      internalNotes: formValue.internalNotes || null
    };

    this.serviceOrderService.create(request).subscribe(result => {
      if (result.isSuccess) {
        this.toastService.successCreate('Ordem de serviço');
        this.closeCreateModal();
        this.loadOrders();
      } else {
        this.toastService.errorCreate('ordem de serviço');
        this.error = result.error || null;
      }
    });
  }
}
