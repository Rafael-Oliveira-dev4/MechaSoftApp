import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BackOfficeRoutingModule } from './back-office-routing.module';
import { BackOfficeLayoutComponent } from './back-office-layout.component';

@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    BackOfficeRoutingModule,
    BackOfficeLayoutComponent
  ]
})
export class BackOfficeModule {}


