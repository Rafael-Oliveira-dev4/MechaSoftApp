/**
 * FRONT-OFFICE ROUTING (Sistema de Gestão)
 *
 * NOTA IMPORTANTE:
 * Este módulo chama-se "front-office" mas É O SISTEMA DE GESTÃO (Admin/Back-Office)
 * Mantemos o nome por compatibilidade, mas representa o sistema para FUNCIONÁRIOS
 *
 * GUARDS:
 * - authGuard: Valida se está autenticado
 * - roleGuard: Valida se tem permissão (role)
 */

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';
import { FrontOfficeLayoutComponent } from './layout/front-office-layout.component';
import { CustomersComponent } from './pages/customers/customers.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { InspectionsComponent } from './pages/inspections/inspections.component';
import { PartsComponent } from './pages/parts/parts.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ServiceOrdersComponent } from './pages/service-orders/service-orders.component';
import { ServicesComponent } from './pages/services/services.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { VehiclesComponent } from './pages/vehicles/vehicles.component';

const routes: Routes = [
  {
    path: '',
    component: FrontOfficeLayoutComponent,
    canActivate: [authGuard], // Requer autenticação
    children: [
      // Dashboard (página inicial do sistema - todos os funcionários)
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },

      // Clientes (Receptionist+)
      {
        path: 'customers',
        component: CustomersComponent,
        canActivate: [roleGuard],
        data: { roles: ['Employee', 'Admin', 'Owner'] },
      },

      // Veículos (todos os funcionários)
      { path: 'vehicles', component: VehiclesComponent },

      // Ordens de Serviço (todos os funcionários)
      { path: 'service-orders', component: ServiceOrdersComponent },

      // Inspeções (todos os funcionários)
      { path: 'inspections', component: InspectionsComponent },

      // Serviços de Oficina (Manager+)
      {
        path: 'services',
        component: ServicesComponent,
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'Owner'] },
      },

      // Peças (PartsClerk+)
      {
        path: 'parts',
        component: PartsComponent,
        canActivate: [roleGuard],
        data: { roles: ['Employee', 'Admin', 'Owner'] },
      },

      // Perfil (todos os utilizadores autenticados)
      { path: 'profile', component: ProfileComponent },

      // Configurações (todos os utilizadores autenticados)
      { path: 'settings', component: SettingsComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FrontOfficeRoutingModule {}
