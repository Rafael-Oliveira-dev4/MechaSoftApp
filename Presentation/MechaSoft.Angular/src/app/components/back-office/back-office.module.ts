import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { BackOfficeRoutingModule } from './back-office-routing.module';
import { BackOfficeLayoutComponent } from './layout/back-office-layout.component';
import { CustomersComponent } from './pages/customers/customers.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { InspectionsComponent } from './pages/inspections/inspections.component';
import { PartsComponent } from './pages/parts/parts.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ServiceOrdersComponent } from './pages/service-orders/service-orders.component';
import { RequestServiceOrderComponent } from './pages/service-orders/request-service-order.component';
import { BookInspectionComponent } from './pages/inspections/book-inspection.component';
import { ServicesComponent } from './pages/services/services.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { VehiclesComponent } from './pages/vehicles/vehicles.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    BackOfficeRoutingModule,
    BackOfficeLayoutComponent,
    DashboardComponent,
    CustomersComponent,
    VehiclesComponent,
    ServicesComponent,
    PartsComponent,
    InspectionsComponent,
    ServiceOrdersComponent,
    RequestServiceOrderComponent,
    BookInspectionComponent,
    ProfileComponent,
    SettingsComponent,
  ],
})
export class BackOfficeModule {}
