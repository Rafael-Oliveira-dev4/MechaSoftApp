import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { VehicleService } from '../../../../core/services/vehicle.service';
import { ToastService } from '../../../../core/services/toast.service';
import { Vehicle } from '../../../../core/models/api.models';
import { Result } from '../../../../core/models/result.model';

@Component({
  selector: 'app-book-inspection',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './book-inspection.component.html',
  styleUrls: ['./book-inspection.component.scss'],
})
export class BookInspectionComponent implements OnInit {
  step = 1;
  maxStep = 3;
  vehicles: Vehicle[] = [];
  loading = false;
  submitting = false;
  error: string | null = null;

  form: FormGroup;

  inspectionTypes = [
    { value: 'Periodic', label: 'Inspeção periódica' },
    { value: 'Extraordinary', label: 'Inspeção extraordinária' },
    { value: 'Recheck', label: 'Reinspeção' },
  ];

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private vehicleService: VehicleService,
    private toast: ToastService,
    private router: Router
  ) {
    this.form = this.fb.group({
      vehicleId: [null, Validators.required],
      type: ['Periodic', Validators.required],
      preferredDate: [null, Validators.required],
      observations: [''],
    });
  }

  ngOnInit(): void {
    this.loadVehicles();
  }

  private loadVehicles(): void {
    const customerId = this.auth.getCurrentUser()?.customerId;
    if (!customerId) {
      this.error = 'Não foi possível identificar o cliente. Faça login novamente.';
      return;
    }
    this.loading = true;
    this.error = null;
    this.vehicleService.getByCustomer(customerId).subscribe({
      next: (result: Result<Vehicle[]>) => {
        if (result.isSuccess && result.value) {
          this.vehicles = result.value;
          if (this.vehicles.length === 0) {
            this.error = 'Não tem veículos registados. Registe um veículo primeiro.';
          }
        } else {
          this.error = result.error?.message || 'Erro ao carregar veículos.';
        }
        this.loading = false;
      },
      error: () => {
        this.error = 'Erro ao carregar veículos.';
        this.loading = false;
      },
    });
  }

  get selectedVehicle(): Vehicle | undefined {
    const id = this.form.get('vehicleId')?.value;
    return id ? this.vehicles.find(v => v.id === id) : undefined;
  }

  get selectedTypeLabel(): string {
    const v = this.form.get('type')?.value;
    return this.inspectionTypes.find(t => t.value === v)?.label || v;
  }

  nextStep(): void {
    if (this.step < this.maxStep) {
      this.step++;
    }
  }

  prevStep(): void {
    if (this.step > 1) {
      this.step--;
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    // Mock: quando existir api/inspection-requests, chamar o serviço aqui
    this.submitting = true;
    this.error = null;
    setTimeout(() => {
      this.submitting = false;
      this.toast.success('Pedido enviado. A oficina confirmará brevemente.');
      this.router.navigate(['/admin/inspections']);
    }, 600);
  }

  formatDate(d: string | Date | null): string {
    if (!d) return '—';
    const date = typeof d === 'string' ? new Date(d) : d;
    return date.toLocaleDateString('pt-PT', { day: '2-digit', month: 'long', year: 'numeric' });
  }
}
