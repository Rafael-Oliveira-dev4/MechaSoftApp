import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PartService } from '../../../../core/services/part.service';
import { Part, CreatePartRequest, UpdatePartRequest } from '../../../../core/models/part.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-parts',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, ErrorMessageComponent],
  templateUrl: './parts.component.html',
  styleUrls: ['./parts.component.scss']
})
export class PartsComponent implements OnInit {
  parts: Part[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  showLowStockOnly: boolean = false;
  searchTerm: string = '';
  
  showModal: boolean = false;
  showStockModal: boolean = false;
  isEditMode: boolean = false;
  selectedPartId: string | null = null;
  selectedPart: Part | null = null;
  
  partForm: FormGroup;
  stockForm: FormGroup;
  error: ErrorDetail | null = null;
  loading$;

  constructor(
    private partService: PartService,
    private loadingService: LoadingService,
    private fb: FormBuilder
  ) {
    this.partForm = this.createPartForm();
    this.stockForm = this.createStockForm();
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadParts();
  }

  // Criar formulário de peça
  private createPartForm(): FormGroup {
    return this.fb.group({
      partNumber: ['', [Validators.required, Validators.minLength(3)]],
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      brand: ['', Validators.required],
      category: ['', Validators.required],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      quantityInStock: [0, [Validators.required, Validators.min(0)]],
      minimumStock: [0, [Validators.required, Validators.min(0)]],
      location: ['', Validators.required],
      supplier: ['']
    });
  }

  // Criar formulário de stock
  private createStockForm(): FormGroup {
    return this.fb.group({
      quantity: [0, [Validators.required, Validators.min(1)]],
      operation: ['add', Validators.required]
    });
  }

  // Carregar peças
  loadParts(): void {
    this.partService.getAll(this.currentPage, this.pageSize, this.showLowStockOnly || undefined).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.parts = result.value.items;
        this.totalCount = result.value.totalCount;
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Abrir modal para criar
  openCreateModal(): void {
    this.isEditMode = false;
    this.selectedPartId = null;
    this.partForm.reset({ quantityInStock: 0, minimumStock: 0 });
    this.showModal = true;
  }

  // Abrir modal para editar
  openEditModal(part: Part): void {
    this.isEditMode = true;
    this.selectedPartId = part.id;
    this.partForm.patchValue({
      partNumber: part.partNumber,
      name: part.name,
      description: part.description,
      brand: part.brand,
      category: part.category,
      unitPrice: part.unitPrice,
      location: part.location,
      supplier: part.supplier
    });
    this.showModal = true;
  }

  // Abrir modal de stock
  openStockModal(part: Part): void {
    this.selectedPart = part;
    this.stockForm.reset({ quantity: 0, operation: 'add' });
    this.showStockModal = true;
  }

  // Fechar modais
  closeModal(): void {
    this.showModal = false;
    this.partForm.reset();
    this.error = null;
  }

  closeStockModal(): void {
    this.showStockModal = false;
    this.selectedPart = null;
    this.stockForm.reset();
    this.error = null;
  }

  // Submeter formulário de peça
  onSubmit(): void {
    if (this.partForm.invalid) {
      this.partForm.markAllAsTouched();
      return;
    }

    const request = this.partForm.value;

    const operation$ = this.isEditMode && this.selectedPartId
      ? this.partService.update(this.selectedPartId, request as UpdatePartRequest)
      : this.partService.create(request as CreatePartRequest);

    operation$.subscribe(result => {
      if (result.isSuccess) {
        this.closeModal();
        this.loadParts();
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Submeter atualização de stock
  onUpdateStock(): void {
    if (this.stockForm.invalid || !this.selectedPart) return;

    const { quantity, operation } = this.stockForm.value;

    this.partService.updateStock(this.selectedPart.id, quantity, operation).subscribe(result => {
      if (result.isSuccess) {
        this.closeStockModal();
        this.loadParts();
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Toggle active/inactive
  toggleActive(part: Part): void {
    if (confirm(`Deseja ${part.isActive ? 'desativar' : 'ativar'} a peça ${part.name}?`)) {
      this.partService.toggleActive(part.id).subscribe(result => {
        if (result.isSuccess) {
          this.loadParts();
        } else {
          this.error = result.error || null;
        }
      });
    }
  }

  // Toggle filtro de low stock
  toggleLowStock(): void {
    this.showLowStockOnly = !this.showLowStockOnly;
    this.currentPage = 1;
    this.loadParts();
  }

  // Paginação
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadParts();
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  // Verificar se stock está baixo
  isLowStock(part: Part): boolean {
    return part.quantityInStock <= part.minimumStock;
  }

  // Helpers de validação
  isFieldInvalid(fieldName: string, form: FormGroup = this.partForm): boolean {
    const field = form.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string, form: FormGroup = this.partForm): string {
    const field = form.get(fieldName);
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
