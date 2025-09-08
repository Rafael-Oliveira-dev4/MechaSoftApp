import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorComponent } from './components/common/error/error.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./components/front-office/front-office.module').then(m => m.FrontOfficeModule)
  },
  {
    path: 'admin',
    loadChildren: () => import('./components/back-office/back-office.module').then(m => m.BackOfficeModule)
  },
  { path: '404', component: ErrorComponent },
  { path: '**', redirectTo: '404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
