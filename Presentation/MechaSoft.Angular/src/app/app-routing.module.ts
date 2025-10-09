import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorComponent } from './shared/components/error/error.component';
import { LoginComponent } from './components/auth/login/login.component';
import { LandingComponent } from './components/landing/landing.component';

const routes: Routes = [
  // Landing Page (público)
  { path: '', component: LandingComponent },
  
  // Login (público)
  { path: 'login', component: LoginComponent },
  
  // Sistema de Gestão (Front-Office = Admin System)
  // NOTA: Nome "front-office" é mantido por compatibilidade
  // Este é o SISTEMA DE GESTÃO para funcionários
  {
    path: 'app',
    loadChildren: () => import('./components/front-office/front-office.module').then(m => m.FrontOfficeModule)
  },
  
  // Back-Office (reservado para futuro)
  {
    path: 'admin',
    loadChildren: () => import('./components/back-office/back-office.module').then(m => m.BackOfficeModule)
  },
  
  // Error pages
  { path: '404', component: ErrorComponent },
  { path: '**', redirectTo: '404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
