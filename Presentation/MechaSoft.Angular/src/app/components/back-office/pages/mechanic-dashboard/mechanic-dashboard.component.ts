import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { User } from '../../../../core/models/api.models';
import { MechanicDashboardStats } from '../../../../core/models/dashboard.model';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { ErrorDetail } from '../../../../core/models/result.model';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-mechanic-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, ErrorMessageComponent],
  templateUrl: './mechanic-dashboard.component.html',
  styleUrls: ['./mechanic-dashboard.component.scss'],
})
export class MechanicDashboardComponent implements OnInit {
  currentUser: User | null = null;
  stats: MechanicDashboardStats | null = null;
  error: ErrorDetail | null = null;

  constructor(
    private authService: AuthService,
    private dashboardService: DashboardService
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      if (user?.employeeId) {
        this.loadDashboardData();
      } else {
        this.error = { code: 'NO_EMPLOYEE', message: 'Utilizador sem perfil de mecânico associado.' };
      }
    });
  }

  private loadDashboardData(): void {
    if (!this.currentUser?.employeeId) return;
    this.error = null;
    this.dashboardService.getMechanicStats(this.currentUser.employeeId).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.stats = result.value;
      } else {
        this.error = result.error || { code: 'ERROR', message: 'Erro ao carregar dados.' };
      }
    });
  }

  refresh(): void {
    this.error = null;
    this.loadDashboardData();
  }

  formatDate(value: Date | string | null): string {
    if (value == null) return '-';
    const d = typeof value === 'string' ? new Date(value) : value;
    return isNaN(d.getTime()) ? '-' : d.toLocaleDateString('pt-PT', { day: '2-digit', month: '2-digit', year: 'numeric' });
  }

  getStatusLabel(status: string): string {
    const map: Record<string, string> = {
      Pending: 'Pendente',
      InProgress: 'Em curso',
      WaitingParts: 'À espera de peças',
      WaitingInspection: 'À espera de inspeção',
      Completed: 'Concluída',
    };
    return map[status] || status;
  }
}
