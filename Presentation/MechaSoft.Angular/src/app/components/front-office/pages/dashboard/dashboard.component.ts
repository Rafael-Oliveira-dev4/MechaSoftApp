import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { DashboardStats, LowStockReport } from '../../../../core/models/dashboard.model';
import { ErrorDetail } from '../../../../core/models/result.model';
import { LoadingService } from '../../../../core/services/loading.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ErrorMessageComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  stats: DashboardStats | null = null;
  lowStockReport: LowStockReport | null = null;
  error: ErrorDetail | null = null;
  loading$;

  constructor(
    private dashboardService: DashboardService,
    private loadingService: LoadingService
  ) {
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  // Carregar dados do dashboard
  private loadDashboardData(): void {
    this.dashboardService.getStats().subscribe(result => {
      if (result.isSuccess && result.value) {
        this.stats = result.value;
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

  // Atualizar dados
  refresh(): void {
    this.error = null;
    this.loadDashboardData();
  }

  // Formatar moeda
  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-PT', {
      style: 'currency',
      currency: 'EUR'
    }).format(value);
  }

  // Obter cor do badge de status
  getStatusColor(status: string): string {
    const statusMap: { [key: string]: string } = {
      'Pending': 'yellow',
      'InProgress': 'blue',
      'Completed': 'green',
      'Cancelled': 'red'
    };
    return statusMap[status] || 'gray';
  }
}
