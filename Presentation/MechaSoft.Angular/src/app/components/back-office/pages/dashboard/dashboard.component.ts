import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/services/auth.service';
import { User } from '../../../../core/models/api.models';
import { AdminDashboardComponent } from '../admin-dashboard/admin-dashboard.component';
import { CustomerDashboardComponent } from '../customer-dashboard/customer-dashboard.component';
import { MechanicDashboardComponent } from '../mechanic-dashboard/mechanic-dashboard.component';

/**
 * DashboardComponent - Wrapper inteligente
 *
 * Decide qual dashboard mostrar baseado na role do utilizador:
 * - Customer → CustomerDashboardComponent
 * - Mechanic → MechanicDashboardComponent
 * - Owner/Admin/Employee → AdminDashboardComponent
 */
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, AdminDashboardComponent, CustomerDashboardComponent, MechanicDashboardComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  currentUser: User | null = null;
  isCustomer = false;
  isMechanic = false;
  isAdmin = false;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      this.isCustomer = user?.role === 'Customer';
      this.isMechanic = user?.role === 'Mechanic';
      this.isAdmin = user?.role === 'Owner' || user?.role === 'Admin' || user?.role === 'Employee';
    });
  }
}
