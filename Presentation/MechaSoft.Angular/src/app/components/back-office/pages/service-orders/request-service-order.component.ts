import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { VehicleService } from '../../../../core/services/vehicle.service';
import { ServiceOrderService } from '../../../../core/services/service-order.service';
import { ToastService } from '../../../../core/services/toast.service';
import { Vehicle } from '../../../../core/models/api.models';
import { Result } from '../../../../core/models/result.model';

@Component({
  selector: 'app-request-service-order',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './request-service-order.component.html',
  styleUrls: ['./request-service-order.component.scss'],
})
export class RequestServiceOrderComponent implements OnInit {
  step = 1;
  maxStep = 3;
  vehicles: Vehicle[] = [];
  loading = false;
  submitting = false;
  error: string | null = null;

  form: FormGroup;

  priorities = [
    { value: 'Low', label: 'Baixa' },
    { value: 'Medium', label: 'Média' },
    { value: 'High', label: 'Alta' },
    { value: 'Urgent', label: 'Urgente' },
  ];

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private vehicleService: VehicleService,
    private serviceOrderService: ServiceOrderService,
    private toast: ToastService,
    private router: Router
  ) {
    this.form = this.fb.group({
      vehicleId: [null, Validators.required],
      description: ['', [Validators.required, Validators.minLength(10)]],
      priority: ['Medium', Validators.required],
      preferredDate: [null],
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
    const customerId = this.auth.getCurrentUser()?.customerId;
    if (!customerId) {
      this.toast.error('Sessão inválida. Faça login novamente.');
      return;
    }
    const v = this.form.value;
    this.submitting = true;
    this.error = null;
    this.serviceOrderService.create({
      customerId,
      vehicleId: v.vehicleId,
      description: v.description,
      priority: v.priority,
      estimatedCost: 0,
      estimatedDelivery: v.preferredDate ? new Date(v.preferredDate) : undefined,
      requiresInspection: false,
      internalNotes: v.observations?.trim() || undefined,
    }).subscribe({
      next: (result) => {
        this.submitting = false;
        if (result.isSuccess) {
          this.toast.success('Pedido enviado. A oficina confirmará brevemente.');
          this.router.navigate(['/admin/service-orders']);
        } else {
          this.error = result.error?.message || 'Erro ao enviar pedido.';
        }
      },
      error: () => {
        this.submitting = false;
        this.error = 'Erro ao enviar pedido. Tente novamente.';
      },
    });
  }

  formatDate(d: string | Date | null): string {
    if (!d) return '—';
    const date = typeof d === 'string' ? new Date(d) : d;
    return date.toLocaleDateString('pt-PT', { day: '2-digit', month: 'long', year: 'numeric' });
  }
}
