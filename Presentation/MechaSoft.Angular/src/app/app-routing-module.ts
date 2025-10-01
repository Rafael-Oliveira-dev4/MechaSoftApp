import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorComponent } from './components/common/error/error.component';
import { LoginComponent } from './components/auth/login/login.component';

const routes: Routes = [
  // Login (público)
  { path: 'login', component: LoginComponent },
  
  // Sistema de Gestão (Front-Office = Admin System)
  // NOTA: Nome "front-office" é mantido por compatibilidade
  // Este é o SISTEMA DE GESTÃO para funcionários
  {
    path: '',
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
