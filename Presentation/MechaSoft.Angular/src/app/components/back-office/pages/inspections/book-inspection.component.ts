import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { finalize, takeUntil, timeout } from 'rxjs/operators';
import { AuthService } from '../../../../core/services/auth.service';
import { VehicleService } from '../../../../core/services/vehicle.service';
import { ServiceOrderService } from '../../../../core/services/service-order.service';
import { ToastService } from '../../../../core/services/toast.service';
import { Vehicle } from '../../../../core/models/api.models';
import { Result } from '../../../../core/models/result.model';
import { BookingCalendarComponent } from '../../../../shared/components/booking-calendar/booking-calendar.component';
import { TimeSlotsComponent } from '../../../../shared/components/time-slots/time-slots.component';

const PROFILE_WAIT_MS = 10000;

@Component({
  selector: 'app-book-inspection',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    BookingCalendarComponent,
    TimeSlotsComponent,
  ],
  templateUrl: './book-inspection.component.html',
  styleUrls: ['./book-inspection.component.scss'],
})
export class BookInspectionComponent implements OnInit, OnDestroy {
  step = 1;
  maxStep = 3;
  vehicles: Vehicle[] = [];
  loading = false;
  profileLoading = true;
  submitting = false;
  error: string | null = null;

  form: FormGroup;

  private profileWaitTimeout: ReturnType<typeof setTimeout> | null = null;
  private readonly destroy$ = new Subject<void>();

  inspectionTypes = [
    { value: 'Periodic', label: 'Inspeção periódica' },
    { value: 'Extraordinary', label: 'Inspeção extraordinária' },
    { value: 'Recheck', label: 'Reinspeção' },
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
      type: ['Periodic', Validators.required],
      preferredDate: [null as string | null, Validators.required],
      preferredTime: [null as string | null, Validators.required],
      observations: [''],
    });
  }

  ngOnInit(): void {
    this.auth.currentUser$.pipe(takeUntil(this.destroy$)).subscribe((user) => {
      if (!user) {
        this.clearProfileWait();
        this.profileLoading = false;
        this.toast.error('Sessão expirada. Inicie sessão novamente.');
        this.router.navigate(['/login'], { queryParams: { returnUrl: '/admin/inspections/book' } });
        return;
      }
      if (user.customerId) {
        this.clearProfileWait();
        this.profileLoading = false;
        this.loadVehicles(user.customerId);
        return;
      }
      if (user.customerId == null) {
        if (this.profileWaitTimeout !== null) return;
        this.profileWaitTimeout = setTimeout(() => {
          this.profileWaitTimeout = null;
          this.profileLoading = false;
          this.error = 'Não foi possível identificar o cliente. Faça login novamente.';
        }, PROFILE_WAIT_MS);
        return;
      }
      this.clearProfileWait();
      this.profileLoading = false;
      this.error = null;
    });
  }

  ngOnDestroy(): void {
    this.clearProfileWait();
    this.destroy$.next();
    this.destroy$.complete();
  }

  private clearProfileWait(): void {
    if (this.profileWaitTimeout !== null) {
      clearTimeout(this.profileWaitTimeout);
      this.profileWaitTimeout = null;
    }
  }

  private loadVehicles(customerId: string): void {
    this.loading = true;
    this.error = null;
    this.vehicleService.getByCustomer(customerId).pipe(
      timeout(15000),
      takeUntil(this.destroy$),
      finalize(() => { this.loading = false; })
    ).subscribe({
      next: (result: Result<Vehicle[]>) => {
        if (result.isSuccess && result.value) {
          this.vehicles = result.value;
          if (this.vehicles.length === 0) {
            this.error = 'Não tem veículos registados. Registe um veículo primeiro.';
          }
        } else {
          this.error = result.error?.message || 'Erro ao carregar veículos.';
        }
      },
      error: (err: unknown) => {
        const isTimeout = err && typeof err === 'object' && ('name' in err && (err as { name?: string }).name === 'TimeoutError');
        this.error = isTimeout
          ? 'Demorou demasiado a carregar. Tente novamente.'
          : 'Erro ao carregar veículos. Tente novamente.';
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

  onDateSelected(date: string): void {
    this.form.patchValue({ preferredDate: date || null, preferredTime: null });
    this.form.get('preferredDate')?.markAsTouched();
  }

  onTimeSelected(time: string): void {
    this.form.patchValue({ preferredTime: time });
    this.form.get('preferredTime')?.markAsTouched();
  }

  get preferredDateTimeISO(): string | null {
    const d = this.form.get('preferredDate')?.value;
    const t = this.form.get('preferredTime')?.value;
    if (!d || !t) return null;
    return `${d}T${t}:00`;
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
      this.router.navigate(['/login'], { queryParams: { returnUrl: '/admin/inspections/book' } });
      return;
    }
    const v = this.form.value;
    const dateTimeLabel = this.formatDateTimeLabel(v.preferredDate, v.preferredTime);
    const description = `Agendamento de inspeção: ${this.selectedTypeLabel}. Data preferida: ${dateTimeLabel}.${v.observations ? ` Observações: ${v.observations}` : ''}`;
    const estimatedDelivery = this.preferredDateTimeISO ? new Date(this.preferredDateTimeISO) : undefined;

    this.submitting = true;
    this.error = null;
    this.serviceOrderService.create({
      customerId,
      vehicleId: v.vehicleId,
      description,
      priority: 'Medium',
      estimatedCost: 0,
      estimatedDelivery,
      requiresInspection: true,
      internalNotes: v.observations?.trim() || undefined,
    }).subscribe({
      next: (result) => {
        this.submitting = false;
        if (result.isSuccess) {
          this.toast.success('Pedido de agendamento enviado. A oficina confirmará brevemente. Pode acompanhar em Ordens.');
          this.router.navigate(['/admin/service-orders']);
        } else {
          this.error = result.error?.message || 'Erro ao enviar pedido. Tente novamente.';
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

  formatTime(t: string | null): string {
    if (!t) return '—';
    const [h, m] = t.split(':');
    return `${h}:${m}`;
  }

  formatDateTimeLabel(dateStr: string | null, timeStr: string | null): string {
    if (!dateStr || !timeStr) return '—';
    return `${this.formatDate(dateStr)} às ${this.formatTime(timeStr)}`;
  }
}
