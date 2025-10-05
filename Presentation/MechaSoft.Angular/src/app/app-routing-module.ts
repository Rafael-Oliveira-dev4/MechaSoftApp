import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { ErrorComponent } from './components/common/error/error.component';
import { FrontHomeComponent } from './components/front-office/pages/home/home.component';

const routes: Routes = [
  // Home (público - acessível a todos)
  { path: '', component: FrontHomeComponent },
  { path: 'home', component: FrontHomeComponent },

  // Auth (Login/Register)
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // Sistema de Gestão (Front-Office = Admin System)
  // NOTA: Nome "front-office" é mantido por compatibilidade
  // Este é o SISTEMA DE GESTÃO para funcionários (protegido)
  {
    path: 'system',
    loadChildren: () =>
      import('./components/front-office/front-office.module').then(m => m.FrontOfficeModule),
  },

  // Back-Office (reservado para futuro)
  {
    path: 'admin',
    loadChildren: () =>
      import('./components/back-office/back-office.module').then(m => m.BackOfficeModule),
  },

  // Error pages
  { path: '404', component: ErrorComponent },
  { path: '**', redirectTo: '404' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
