import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BackOfficeRoutingModule } from './back-office-routing.module';
import { BackOfficeMainComponent } from './main.component';

@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    BackOfficeRoutingModule,
    BackOfficeMainComponent
  ]
})
export class BackOfficeModule {}


