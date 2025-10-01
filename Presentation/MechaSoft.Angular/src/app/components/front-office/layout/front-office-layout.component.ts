import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-front-office-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './front-office-layout.component.html',
  styleUrls: ['./front-office-layout.component.scss']
})
export class FrontOfficeLayoutComponent {
  isMobileMenuOpen = false;

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  closeMobileMenu() {
    this.isMobileMenuOpen = false;
  }
}
