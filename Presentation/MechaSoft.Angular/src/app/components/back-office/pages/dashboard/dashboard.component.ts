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
import { AuthService } from '../../../../core/services/auth.service';
import { User } from '../../../../core/models/api.models';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import { ProfileCompletionAlertComponent } from '../../components/profile-completion-alert/profile-completion-alert.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ErrorMessageComponent, ProfileCompletionAlertComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit, AfterViewInit {
  @ViewChildren('statCard') statCards!: QueryList<ElementRef>;

  stats: DashboardStats | null = null;
  lowStockReport: LowStockReport | null = null;
  error: ErrorDetail | null = null;
  loading$;
  currentUser: User | null = null;
  showProfileAlert = false;

  // Animated values
  animatedTotalOrders: number = 0;
  animatedMonthRevenue: number = 0;
  animatedTotalCustomers: number = 0;
  animatedLowStockCount: number = 0;

  constructor(
    private dashboardService: DashboardService, 
    private loadingService: LoadingService,
    private authService: AuthService
  ) {
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadDashboardData();
    this.checkUserProfile();
  }

  private checkUserProfile(): void {
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      // Show alert if user is Customer and doesn't have a customer profile
      this.showProfileAlert = 
        user?.role === 'Customer' && !user?.customerId;
    });
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
    // TODO: Descomentar quando o backend estiver pronto
    // this.dashboardService.getStats().subscribe(result => {
    //   if (result.isSuccess && result.value) {
    //     this.stats = result.value;
    //     this.animateCounters();
    //   } else {
    //     this.error = result.error || null;
    //   }
    // });
    // this.dashboardService.getLowStockReport().subscribe(result => {
    //   if (result.isSuccess && result.value) {
    //     this.lowStockReport = result.value;
    //   }
    // });

    // Usar dados fictícios para visualização
    this.loadMockData();
  }

  // Dados fictícios para desenvolvimento/visualização (Cliente)
  private loadMockData(): void {
    setTimeout(() => {
      this.stats = {
        totalOrders: 5,
        pendingOrders: 1,
        inProgressOrders: 1,
        completedOrders: 3,
        monthRevenue: 2450.00, // Total gasto pelo cliente este ano
        todayRevenue: 0,
        yearRevenue: 2450.00,
        totalCustomers: 0, // Não usado no dashboard de cliente
        totalVehicles: 2, // Veículos do cliente
        totalPartsValue: 0,
        lowStockPartsCount: 0,
        pendingInspections: 0,
        nextAppointment: {
          date: new Date('2024-10-15T14:00:00'),
          service: 'Revisão dos 10.000 km',
          vehicle: 'BMW 320d'
        },
        monthlyExpenses: [
          { month: 'Mai', amount: 320.50 },
          { month: 'Jun', amount: 180.00 },
          { month: 'Jul', amount: 560.75 },
          { month: 'Ago', amount: 425.00 },
          { month: 'Set', amount: 654.50 },
          { month: 'Out', amount: 310.25 }
        ],
        vehicles: [
          {
            id: '1',
            plate: '12-AB-34',
            brand: 'BMW',
            model: '320d',
            year: 2020,
            lastService: '05/09/2024'
          },
          {
            id: '2',
            plate: '56-CD-78',
            brand: 'Volkswagen',
            model: 'Golf 1.6 TDI',
            year: 2018,
            lastService: '20/08/2024'
          }
        ],
        recentOrders: [
          {
            id: '1',
            orderNumber: 'OS-2024-156',
            vehiclePlate: '12-AB-34',
            service: 'Troca de óleo e filtros',
            status: 'InProgress',
            createdAt: new Date('2024-10-09T10:30:00')
          },
          {
            id: '2',
            orderNumber: 'OS-2024-142',
            vehiclePlate: '56-CD-78',
            service: 'Revisão periódica',
            status: 'Pending',
            createdAt: new Date('2024-09-28T14:15:00')
          },
          {
            id: '3',
            orderNumber: 'OS-2024-098',
            vehiclePlate: '12-AB-34',
            service: 'Substituição de pastilhas',
            status: 'Completed',
            createdAt: new Date('2024-09-05T16:45:00')
          },
          {
            id: '4',
            orderNumber: 'OS-2024-067',
            vehiclePlate: '56-CD-78',
            service: 'Alinhamento e balanceamento',
            status: 'Completed',
            createdAt: new Date('2024-08-20T11:20:00')
          },
          {
            id: '5',
            orderNumber: 'OS-2024-023',
            vehiclePlate: '12-AB-34',
            service: 'Diagnóstico eletrónico',
            status: 'Completed',
            createdAt: new Date('2024-07-15T09:00:00')
          }
        ]
      };

      this.animateCounters();
    }, 500); // Simular delay de rede
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
    this.stats = null; // Reset stats to show loading
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
      InProgress: 'Em Curso',
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

  // Métodos para gráfico de gastos
  getExpensePercentage(amount: number): number {
    if (!this.stats?.monthlyExpenses) return 0;
    const maxExpense = Math.max(...this.stats.monthlyExpenses.map(e => e.amount));
    return (amount / maxExpense) * 100;
  }

  getAverageExpense(): number {
    if (!this.stats?.monthlyExpenses || this.stats.monthlyExpenses.length === 0) return 0;
    const total = this.stats.monthlyExpenses.reduce((sum, e) => sum + e.amount, 0);
    return total / this.stats.monthlyExpenses.length;
  }

  getTotalExpenses(): number {
    if (!this.stats?.monthlyExpenses) return 0;
    return this.stats.monthlyExpenses.reduce((sum, e) => sum + e.amount, 0);
  }
}
