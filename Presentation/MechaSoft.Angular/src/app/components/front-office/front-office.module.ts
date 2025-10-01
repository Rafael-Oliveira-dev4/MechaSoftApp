import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FrontOfficeRoutingModule } from './front-office-routing.module';
import { SharedModule } from '../../shared.module';
import { FrontOfficeLayoutComponent } from './layout/front-office-layout.component';
import { FrontHomeComponent } from './pages/home/home.component';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { CustomersComponent } from './pages/customers/customers.component';
import { VehiclesComponent } from './pages/vehicles/vehicles.component';
import { ServicesComponent } from './pages/services/services.component';
import { PartsComponent } from './pages/parts/parts.component';
import { InspectionsComponent } from './pages/inspections/inspections.component';
import { ServiceOrdersComponent } from './pages/service-orders/service-orders.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    FrontOfficeRoutingModule,
    FrontOfficeLayoutComponent,
    FrontHomeComponent,
    PageHeaderComponent,
    DashboardComponent,
    CustomersComponent,
    VehiclesComponent,
    ServicesComponent,
    PartsComponent,
    InspectionsComponent,
    ServiceOrdersComponent
  ]
})
export class FrontOfficeModule {}


