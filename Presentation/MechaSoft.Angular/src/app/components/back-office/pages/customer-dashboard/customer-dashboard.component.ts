import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { DashboardService, CustomerDashboardStats } from '../../../../core/services/dashboard.service';
import { User } from '../../../../core/models/api.models';
import { ProfileCompletionAlertComponent } from '../../components/profile-completion-alert/profile-completion-alert.component';

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, ProfileCompletionAlertComponent],
  templateUrl: './customer-dashboard.component.html',
  styleUrls: ['./customer-dashboard.component.scss'],
})
export class CustomerDashboardComponent implements OnInit {
  currentUser: User | null = null;
  showProfileAlert = false;
  stats: CustomerDashboardStats | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private authService: AuthService,
    private dashboardService: DashboardService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.checkUserProfile();
    this.loadCustomerData();
  }

  private checkUserProfile(): void {
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      // Show alert if user is Customer and doesn't have a customer profile
      this.showProfileAlert = user?.role === 'Customer' && !user?.customerId;
    });
  }

  private loadCustomerData(): void {
    // Carregar dados reais do cliente logado
    if (!this.currentUser?.customerId) {
      console.warn('CustomerId not found for user', this.currentUser);
      return;
    }

    this.loading = true;
    this.error = null;

    this.dashboardService.getCustomerStats(this.currentUser.customerId).subscribe({
      next: (result) => {
        if (result.isSuccess && result.value) {
          this.stats = result.value;
          this.cdr.detectChanges(); // Force change detection
        } else {
          this.error = result.error?.message || 'Erro ao carregar dados';
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading customer dashboard:', err);
        this.error = 'Erro ao carregar dados do dashboard';
        this.loading = false;
      }
    });
  }

  refresh(): void {
    this.loadCustomerData();
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-PT', {
      style: 'currency',
      currency: 'EUR',
    }).format(value);
  }
}

