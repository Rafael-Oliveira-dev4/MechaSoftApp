import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FrontHomeComponent } from './pages/home/home.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { CustomersComponent } from './pages/customers/customers.component';
import { VehiclesComponent } from './pages/vehicles/vehicles.component';
import { ServicesComponent } from './pages/services/services.component';
import { PartsComponent } from './pages/parts/parts.component';
import { InspectionsComponent } from './pages/inspections/inspections.component';
import { ServiceOrdersComponent } from './pages/service-orders/service-orders.component';

const routes: Routes = [
  { path: '', component: FrontHomeComponent },
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'customers', component: CustomersComponent },
  { path: 'vehicles', component: VehiclesComponent },
  { path: 'services', component: ServicesComponent },
  { path: 'parts', component: PartsComponent },
  { path: 'inspections', component: InspectionsComponent },
  { path: 'service-orders', component: ServiceOrdersComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FrontOfficeRoutingModule {}


