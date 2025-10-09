import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { FrontOfficeRoutingModule } from './front-office-routing.module';
import { FrontOfficeLayoutComponent } from './layout/front-office-layout.component';
import { CustomersComponent } from './pages/customers/customers.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FrontHomeComponent } from './pages/home/home.component';
import { InspectionsComponent } from './pages/inspections/inspections.component';
import { PartsComponent } from './pages/parts/parts.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ServiceOrdersComponent } from './pages/service-orders/service-orders.component';
import { ServicesComponent } from './pages/services/services.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { VehiclesComponent } from './pages/vehicles/vehicles.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    FrontOfficeRoutingModule,
    FrontOfficeLayoutComponent,
    FrontHomeComponent,
    DashboardComponent,
    CustomersComponent,
    VehiclesComponent,
    ServicesComponent,
    PartsComponent,
    InspectionsComponent,
    ServiceOrdersComponent,
    ProfileComponent,
    SettingsComponent,
  ],
})
export class FrontOfficeModule {}
