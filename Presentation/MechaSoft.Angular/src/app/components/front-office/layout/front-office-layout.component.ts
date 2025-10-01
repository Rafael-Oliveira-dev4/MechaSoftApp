import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/api.models';

@Component({
  selector: 'app-front-office-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './front-office-layout.component.html',
  styleUrls: ['./front-office-layout.component.scss']
})
export class FrontOfficeLayoutComponent implements OnInit {
  currentUser: User | null = null;
  isMobileMenuOpen = false;
  
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  // Mobile menu
  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  closeMobileMenu() {
    this.isMobileMenuOpen = false;
  }

  // Logout
  logout(): void {
    if (confirm('Deseja terminar sessão?')) {
      this.authService.logout();
      this.router.navigate(['/login']);
    }
  }

  // Verificar se tem permissão
  hasRole(roles: string[]): boolean {
    return this.currentUser ? roles.includes(this.currentUser.role) : false;
  }
}
