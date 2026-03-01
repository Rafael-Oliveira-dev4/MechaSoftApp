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

const PROFILE_WAIT_MS = 10000; // tempo para o perfil (customerId) chegar após reload

@Component({
  selector: 'app-request-service-order',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    BookingCalendarComponent,
    TimeSlotsComponent,
  ],
  templateUrl: './request-service-order.component.html',
  styleUrls: ['./request-service-order.component.scss'],
})
export class RequestServiceOrderComponent implements OnInit, OnDestroy {
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
      preferredDate: [null as string | null],
      preferredTime: [null as string | null],
      observations: [''],
    });
  }

  ngOnInit(): void {
    console.debug('[RequestServiceOrder] ngOnInit: subscrevendo currentUser$');
    this.auth.currentUser$.pipe(takeUntil(this.destroy$)).subscribe((user) => {
      console.debug('[RequestServiceOrder] currentUser$ emit:', user ? { id: user.id, customerId: user.customerId, role: user.role } : null);
      if (!user) {
        console.debug('[RequestServiceOrder] Sem user -> redirect login');
        this.clearProfileWait();
        this.profileLoading = false;
        this.redirectToLoginWithMessage();
        return;
      }
      if (user.customerId) {
        console.debug('[RequestServiceOrder] user com customerId -> loadVehicles', user.customerId);
        this.clearProfileWait();
        this.profileLoading = false;
        this.loadVehicles(user.customerId);
        return;
      }
      if (user.customerId == null) {
        console.debug('[RequestServiceOrder] user sem customerId -> aguardar profile (timeout', PROFILE_WAIT_MS / 1000, 's). Verifique no Network o pedido GET .../accounts/profile/' + user.id);
        if (this.profileWaitTimeout !== null) return;
        this.profileWaitTimeout = setTimeout(() => {
          this.profileWaitTimeout = null;
          this.profileLoading = false;
          this.error = 'Não foi possível identificar o cliente. Verifique a consola (F12) e o separador Network para ver se o pedido ao perfil falhou. Faça login novamente se necessário.';
          console.warn('[RequestServiceOrder] Timeout: customerId não chegou a tempo. O GET /accounts/profile/{id} completou com sucesso?');
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
    console.debug('[RequestServiceOrder] loadVehicles chamado, customerId:', customerId);
    this.loading = true;
    this.error = null;
    this.vehicleService.getByCustomer(customerId).pipe(
      timeout(15000),
      takeUntil(this.destroy$),
      finalize(() => {
        this.loading = false;
        console.debug('[RequestServiceOrder] loadVehicles finalize (loading=false)');
      })
    ).subscribe({
      next: (result: Result<Vehicle[]>) => {
        console.debug('[RequestServiceOrder] getByCustomer next:', { isSuccess: result.isSuccess, count: result.value?.length ?? 0, error: result.error });
        if (result.isSuccess) {
          this.vehicles = Array.isArray(result.value) ? result.value : [];
          if (this.vehicles.length === 0) {
            this.error = 'Não tem veículos registados. Registe um veículo primeiro.';
          }
        } else {
          this.error = result.error?.message || 'Erro ao carregar veículos.';
        }
      },
      error: (err: unknown) => {
        console.error('[RequestServiceOrder] getByCustomer error:', err);
        if (this.isSessionExpired()) {
          this.redirectToLoginWithMessage();
          return;
        }
        const isTimeout = err && typeof err === 'object' && ('name' in err && (err as { name?: string }).name === 'TimeoutError');
        this.error = isTimeout
          ? 'Demorou demasiado a carregar. Tente novamente.'
          : 'Erro ao carregar veículos. Tente novamente.';
      },
    });
  }

  private isSessionExpired(): boolean {
    return !this.auth.getCurrentUser();
  }

  private redirectToLoginWithMessage(): void {
    this.toast.error('Sessão expirada. Inicie sessão novamente.');
    this.router.navigate(['/login'], {
      queryParams: { returnUrl: '/admin/service-orders/request' },
    });
  }

  get selectedVehicle(): Vehicle | undefined {
    const id = this.form.get('vehicleId')?.value;
    return id ? this.vehicles.find(v => v.id === id) : undefined;
  }

  get priorityLabel(): string {
    const value = this.form.get('priority')?.value;
    return this.priorities.find(p => p.value === value)?.label ?? value ?? '—';
  }

  onDateSelected(date: string): void {
    this.form.patchValue({ preferredDate: date || null, preferredTime: null });
  }

  onTimeSelected(time: string): void {
    this.form.patchValue({ preferredTime: time });
  }

  formatDateTimeLabel(dateStr: string | null, timeStr: string | null): string {
    if (!dateStr) return '—';
    const d = this.formatDate(dateStr);
    if (!timeStr) return d;
    return `${d} às ${timeStr}`;
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
      this.redirectToLoginWithMessage();
      return;
    }
    const v = this.form.value;
    this.submitting = true;
    this.error = null;
    const estimatedDelivery = v.preferredDate && v.preferredTime
      ? new Date(`${v.preferredDate}T${v.preferredTime}:00`)
      : v.preferredDate
        ? new Date(v.preferredDate)
        : undefined;
    this.serviceOrderService.create({
      customerId,
      vehicleId: v.vehicleId,
      description: v.description,
      priority: v.priority,
      estimatedCost: 0,
      estimatedDelivery,
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
        if (this.isSessionExpired()) {
          this.redirectToLoginWithMessage();
          return;
        }
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
