import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { DashboardStats } from '../../../../core/models/dashboard.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { LoadingService } from '../../../../core/services/loading.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, ErrorMessageComponent],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
})
export class AdminDashboardComponent implements OnInit {
  stats: DashboardStats | null = null;
  error: ErrorDetail | null = null;
  loading$;

  // Animated values
  animatedTotalCustomers = 0;
  animatedTotalOrders = 0;
  animatedInventoryValue = 0;
  animatedLowStockCount = 0;

  constructor(
    private dashboardService: DashboardService,
    private loadingService: LoadingService,
    private cdr: ChangeDetectorRef
  ) {
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    this.dashboardService.getStats().subscribe(result => {
      if (result.isSuccess && result.value) {
        this.stats = result.value;
        this.cdr.detectChanges(); // Force change detection
        this.animateCounters();
      } else {
        this.error = result.error || null;
        this.cdr.detectChanges();
      }
    });
  }

  private animateCounters(): void {
    if (!this.stats) return;

    const duration = 1500;
    const steps = 60;
    const stepDuration = duration / steps;

    const animate = (target: number, setter: (val: number) => void, isDecimal = false) => {
      const increment = target / steps;
      let step = 0;

      const interval = setInterval(() => {
        step++;
        const value = Math.min(increment * step, target);
        setter(isDecimal ? value : Math.round(value));

        if (step >= steps) {
          clearInterval(interval);
          setter(isDecimal ? target : Math.round(target)); // Ensure final value is exact
        }
      }, stepDuration);
    };

    animate(this.stats.totalCustomers, val => (this.animatedTotalCustomers = val));
    animate(this.stats.serviceOrders?.total || 0, val => (this.animatedTotalOrders = val));
    animate(this.stats.parts?.totalInventoryValue || 0, val => (this.animatedInventoryValue = val), true);
    animate(this.stats.parts?.lowStockParts || 0, val => (this.animatedLowStockCount = val));
  }

  refresh(): void {
    this.error = null;
    this.stats = null;
    this.loadDashboardData();
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-PT', {
      style: 'currency',
      currency: 'EUR',
    }).format(value);
  }
}

