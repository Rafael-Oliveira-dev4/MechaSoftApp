import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CustomerService } from '../../../../core/services/customer.service';
import { Customer, CreateCustomerRequest } from '../../../../core/models/api.models';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, ErrorMessageComponent],
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {
  customers: Customer[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  searchTerm: string = '';
  
  showModal: boolean = false;
  isEditMode: boolean = false;
  selectedCustomerId: string | null = null;
  
  customerForm: FormGroup;
  error: ErrorDetail | null = null;
  loading$;

  constructor(
    private customerService: CustomerService,
    private loadingService: LoadingService,
    private fb: FormBuilder
  ) {
    this.customerForm = this.createForm();
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadCustomers();
  }

  // Criar formulário com validações
  private createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^[0-9]{9}$/)]],
      nif: ['', [Validators.pattern(/^[0-9]{9}$/)]],
      street: ['', Validators.required],
      number: ['', Validators.required],
      parish: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', [Validators.required, Validators.pattern(/^[0-9]{4}-[0-9]{3}$/)]],
      country: ['Portugal', Validators.required]
    });
  }

  // Carregar clientes
  loadCustomers(): void {
    this.customerService.getAll({
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm || undefined
    }).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.customers = result.value.items;
        this.totalCount = result.value.totalCount;
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Abrir modal para criar
  openCreateModal(): void {
    this.isEditMode = false;
    this.selectedCustomerId = null;
    this.customerForm.reset({ country: 'Portugal' });
    this.showModal = true;
  }

  // Abrir modal para editar
  openEditModal(customer: Customer): void {
    this.isEditMode = true;
    this.selectedCustomerId = customer.id;
    this.customerForm.patchValue({
      name: customer.name,
      email: customer.email,
      phone: customer.phone,
      nif: customer.nif,
      street: customer.street,
      number: customer.number,
      parish: customer.parish,
      city: customer.city,
      postalCode: customer.postalCode,
      country: customer.country
    });
    this.showModal = true;
  }

  // Fechar modal
  closeModal(): void {
    this.showModal = false;
    this.customerForm.reset();
    this.error = null;
  }

  // Submeter formulário
  onSubmit(): void {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    const request: CreateCustomerRequest = this.customerForm.value;

    const operation$ = this.isEditMode && this.selectedCustomerId
      ? this.customerService.update(this.selectedCustomerId, request)
      : this.customerService.create(request);

    operation$.subscribe(result => {
      if (result.isSuccess) {
        this.closeModal();
        this.loadCustomers();
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Toggle active/inactive
  toggleActive(customer: Customer): void {
    if (confirm(`Deseja ${customer.isActive ? 'desativar' : 'ativar'} o cliente ${customer.name}?`)) {
      this.customerService.toggleActive(customer.id).subscribe(result => {
        if (result.isSuccess) {
          this.loadCustomers();
        } else {
          this.error = result.error || null;
        }
      });
    }
  }

  // Pesquisar
  onSearch(term: string): void {
    this.searchTerm = term;
    this.currentPage = 1;
    this.loadCustomers();
  }

  // Paginação
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadCustomers();
  }

  // Obter número total de páginas
  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // Helper para validação de formulário
  isFieldInvalid(fieldName: string): boolean {
    const field = this.customerForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  // Obter erro do campo
  getFieldError(fieldName: string): string {
    const field = this.customerForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'Campo obrigatório';
    if (field.errors['email']) return 'Email inválido';
    if (field.errors['minlength']) return `Mínimo ${field.errors['minlength'].requiredLength} caracteres`;
    if (field.errors['pattern']) return 'Formato inválido';
    
    return 'Campo inválido';
  }
}
