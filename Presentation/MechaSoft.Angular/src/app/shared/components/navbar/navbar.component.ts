import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { filter } from 'rxjs/operators';
import { User } from '../../../core/models/api.models';
import { AuthService } from '../../../core/services/auth.service';
import { ProfileImageService } from '../../../core/services/profile-image.service';

interface NavLink {
  label: string;
  path: string;
  icon: string;
  roles?: string[]; // Se vazio, acessível a todos autenticados
  exact?: boolean;
}

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  isAuthenticated = false;
  isMobileMenuOpen = false;
  isUserMenuOpen = false;
  currentRoute = '';

  private destroy$ = new Subject<void>();

  // Links públicos (quando não está autenticado)
  publicLinks: NavLink[] = [
    {
      label: 'Início',
      path: '/',
      icon: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6',
      exact: true,
    },
    {
      label: 'Serviços',
      path: '/#services',
      icon: 'M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z M15 12a3 3 0 11-6 0 3 3 0 016 0z',
    },
  ];

  // Links do sistema (quando autenticado)
  systemLinks: NavLink[] = [
    {
      label: 'Dashboard',
      path: '/admin/dashboard',
      icon: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6',
    },
    {
      label: 'Veículos',
      path: '/admin/vehicles',
      icon: 'M8 7v8a2 2 0 002 2h6M8 7V5a2 2 0 012-2h4.586a1 1 0 01.707.293l4.414 4.414a1 1 0 01.293.707V15a2 2 0 01-2 2h-2M8 7H6a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2v-2',
    },
    {
      label: 'Ordens',
      path: '/admin/service-orders',
      icon: 'M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01',
    },
    {
      label: 'Inspeções',
      path: '/admin/inspections',
      icon: 'M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4',
    },
    // Admin only links
    {
      label: 'Clientes',
      path: '/admin/customers',
      icon: 'M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z',
      roles: ['Employee', 'Admin', 'Owner'],
    },
    {
      label: 'Serviços',
      path: '/admin/services',
      icon: 'M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z',
      roles: ['Admin', 'Owner'],
    },
    {
      label: 'Peças',
      path: '/admin/parts',
      icon: 'M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4',
      roles: ['Employee', 'Admin', 'Owner'],
    },
  ];

  constructor(
    private authService: AuthService,
    private router: Router,
    private profileImageService: ProfileImageService
  ) {}

  ngOnInit(): void {
    // Subscrever ao estado de autenticação
    this.authService.currentUser$.pipe(takeUntil(this.destroy$)).subscribe((user: User | null) => {
      this.currentUser = user;
      this.isAuthenticated = !!user;
    });

    // Rastrear mudanças de rota
    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe((event: NavigationEnd) => {
        this.currentRoute = event.urlAfterRedirects;
        this.isMobileMenuOpen = false;
        this.isUserMenuOpen = false;
      });

    // Rota inicial
    this.currentRoute = this.router.url;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Verificar se o utilizador tem acesso ao link
  hasAccess(link: NavLink): boolean {
    if (!link.roles || link.roles.length === 0) {
      return true; // Link acessível a todos autenticados
    }
    return this.currentUser?.role ? link.roles.includes(this.currentUser.role) : false;
  }

  // Obter links visíveis baseado na autenticação e role
  get visibleSystemLinks(): NavLink[] {
    return this.systemLinks.filter(link => this.hasAccess(link));
  }

  // Toggle mobile menu
  toggleMobileMenu(): void {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
    if (this.isMobileMenuOpen) {
      this.isUserMenuOpen = false;
    }
  }

  // Toggle user dropdown
  toggleUserMenu(): void {
    this.isUserMenuOpen = !this.isUserMenuOpen;
  }

  // Fechar menus
  closeMenus(): void {
    this.isMobileMenuOpen = false;
    this.isUserMenuOpen = false;
  }

  // Logout
  logout(): void {
    if (confirm('Deseja terminar sessão?')) {
      this.authService.logout();
      this.router.navigate(['/login']);
      this.closeMenus();
    }
  }

  // Obter iniciais do nome do utilizador
  getUserInitials(): string {
    try {
      // Verificação mais robusta
      if (!this.currentUser || !this.currentUser.username) {
        return '?';
      }

      const username = String(this.currentUser.username).trim();
      if (!username || username.length === 0) {
        return '?';
      }

      return username.substring(0, 2).toUpperCase();
    } catch (error) {
      console.warn('Error in getUserInitials:', error);
      return '?';
    }
  }

  // URL do avatar (imagem ou fallback)
  getAvatarUrl(): string {
    return this.profileImageService.getProfileImageUrl(this.currentUser?.profileImageUrl);
  }

  // Obter cor do avatar baseado na role
  getAvatarColor(): string {
    if (!this.currentUser?.role) return 'avatar-default';

    const roleColors: { [key: string]: string } = {
      Owner: 'avatar-owner',
      Admin: 'avatar-admin',
      Employee: 'avatar-employee',
      Customer: 'avatar-customer',
    };

    return roleColors[this.currentUser.role] || 'avatar-default';
  }

  // Traduzir role para português
  getRoleLabel(): string {
    if (!this.currentUser?.role) return '';

    const roleLabels: { [key: string]: string } = {
      Owner: 'Proprietário',
      Admin: 'Administrador',
      Employee: 'Funcionário',
      Customer: 'Cliente',
    };

    return roleLabels[this.currentUser.role] || this.currentUser.role;
  }

  // Verificar se está na rota
  isActiveRoute(path: string, exact: boolean = false): boolean {
    if (path.startsWith('/#')) return false; // Âncoras
    if (exact) {
      return this.currentRoute === path;
    }
    return this.currentRoute.startsWith(path);
  }

  // Handler para erro no carregamento da imagem do avatar
  onAvatarError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.style.display = 'none'; // Esconder imagem quebrada
  }
}
