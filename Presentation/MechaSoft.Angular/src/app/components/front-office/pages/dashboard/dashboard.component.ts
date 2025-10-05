import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { DashboardStats, LowStockReport } from '../../../../core/models/dashboard.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { LoadingService } from '../../../../core/services/loading.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ErrorMessageComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit, AfterViewInit {
  @ViewChildren('statCard') statCards!: QueryList<ElementRef>;

  stats: DashboardStats | null = null;
  lowStockReport: LowStockReport | null = null;
  error: ErrorDetail | null = null;
  loading$;

  // Animated values
  animatedTotalOrders: number = 0;
  animatedMonthRevenue: number = 0;
  animatedTotalCustomers: number = 0;
  animatedLowStockCount: number = 0;

  constructor(private dashboardService: DashboardService, private loadingService: LoadingService) {
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  ngAfterViewInit(): void {
    this.observeCards();
  }

  // Observar cards para animação
  private observeCards(): void {
    const observer = new IntersectionObserver(
      entries => {
        entries.forEach(entry => {
          if (entry.isIntersecting) {
            entry.target.classList.add('fade-in-up');
          }
        });
      },
      { threshold: 0.1 }
    );

    this.statCards?.forEach(card => {
      observer.observe(card.nativeElement);
    });
  }

  // Carregar dados do dashboard
  private loadDashboardData(): void {
    this.dashboardService.getStats().subscribe(result => {
      if (result.isSuccess && result.value) {
        this.stats = result.value;
        this.animateCounters();
      } else {
        this.error = result.error || null;
      }
    });

    this.dashboardService.getLowStockReport().subscribe(result => {
      if (result.isSuccess && result.value) {
        this.lowStockReport = result.value;
      }
    });
  }

  // Animar contadores
  private animateCounters(): void {
    if (!this.stats) return;

    const duration = 1500; // 1.5 segundos
    const steps = 60;
    const stepDuration = duration / steps;

    const animate = (current: number, target: number, setter: (val: number) => void) => {
      const increment = target / steps;
      let step = 0;

      const interval = setInterval(() => {
        step++;
        const value = Math.min(current + increment * step, target);
        setter(value);

        if (step >= steps) {
          clearInterval(interval);
        }
      }, stepDuration);
    };

    animate(0, this.stats.totalOrders, val => (this.animatedTotalOrders = Math.round(val)));
    animate(0, this.stats.monthRevenue, val => (this.animatedMonthRevenue = val));
    animate(0, this.stats.totalCustomers, val => (this.animatedTotalCustomers = Math.round(val)));
    animate(
      0,
      this.stats.lowStockPartsCount,
      val => (this.animatedLowStockCount = Math.round(val))
    );
  }

  // Atualizar dados
  refresh(): void {
    this.error = null;
    this.loadDashboardData();
  }

  // Formatar moeda
  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-PT', {
      style: 'currency',
      currency: 'EUR',
    }).format(value);
  }

  // Obter cor do badge de status
  getStatusColor(status: string): string {
    const statusMap: { [key: string]: string } = {
      Pending: 'yellow',
      InProgress: 'blue',
      Completed: 'green',
      Cancelled: 'red',
    };
    return statusMap[status] || 'gray';
  }

  // Obter label de status em português
  getStatusLabel(status: string): string {
    const statusMap: { [key: string]: string } = {
      Pending: 'Pendente',
      InProgress: 'Em Progresso',
      Completed: 'Concluída',
      Cancelled: 'Cancelada',
    };
    return statusMap[status] || status;
  }

  // Calcular percentagem de progresso das ordens
  getOrdersProgress(): number {
    if (!this.stats || this.stats.totalOrders === 0) return 0;
    return (this.stats.completedOrders / this.stats.totalOrders) * 100;
  }

  // Calcular percentagem de stock crítico
  getStockHealthPercentage(): number {
    if (!this.stats) return 100;
    const totalParts = 100; // Assumindo 100 peças no total
    const healthyParts = totalParts - this.stats.lowStockPartsCount;
    return (healthyParts / totalParts) * 100;
  }
}
