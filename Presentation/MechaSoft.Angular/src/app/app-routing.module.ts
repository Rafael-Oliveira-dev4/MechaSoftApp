import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorComponent } from './shared/components/error/error.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { LandingComponent } from './components/landing/landing.component';

const routes: Routes = [
  // Landing Page (público)
  { path: '', component: LandingComponent },
  
  // Login (público)
  { path: 'login', component: LoginComponent },
  
  // Register (público)
  { path: 'register', component: RegisterComponent },
  
  // Back-Office / Admin (Sistema de Gestão Administrativo)
  // Sistema completo para funcionários e administradores
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
