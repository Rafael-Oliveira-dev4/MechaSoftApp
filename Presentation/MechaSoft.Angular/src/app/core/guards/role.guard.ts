import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

// Role Guard - Valida permissões baseado em roles
export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const currentUser = authService.getCurrentUser();
  
  if (!currentUser) {
    router.navigate(['/login']);
    return false;
  }

  // Obter roles permitidas da rota (data: { roles: ['Admin', 'Owner'] })
  const allowedRoles = route.data['roles'] as string[] | undefined;
  
  // Se não tem restrição de roles, permite
  if (!allowedRoles || allowedRoles.length === 0) {
    return true;
  }

  // Verificar se user tem role permitida
  const hasRole = allowedRoles.includes(currentUser.role);
  
  if (!hasRole) {
    // Sem permissão - redirect para dashboard
    router.navigate(['/dashboard']);
    return false;
  }

  return true;
};

