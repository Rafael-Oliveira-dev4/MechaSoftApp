import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InspectionService, Inspection } from '../../../../core/services/inspection.service';
import { AuthService } from '../../../../core/services/auth.service';
import { ErrorDetail } from '../../../../core/models/result.model';

@Component({
  selector: 'app-inspections',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './inspections.component.html',
  styleUrls: ['./inspections.component.scss']
})
export class InspectionsComponent implements OnInit {
  inspections: Inspection[] = [];
  upcomingInspections: Inspection[] = [];
  completedInspections: Inspection[] = [];
  overdueInspections: Inspection[] = [];

  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  error: ErrorDetail | null = null;

  constructor(
    private inspectionService: InspectionService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadInspections();
  }

  // Carregar inspeções da API
  loadInspections(): void {
    const currentUser = this.authService.getCurrentUser();
    const vehicleId = currentUser?.role === 'Customer' ? undefined : undefined; // Admin vê todas

    this.inspectionService.getAll(this.currentPage, this.pageSize, vehicleId).subscribe(result => {
      if (result.isSuccess && result.value) {
        this.inspections = result.value.items;
        this.totalCount = result.value.totalCount;
        this.categorizeInspections();
        this.cdr.detectChanges();
      } else {
        this.error = result.error || null;
      }
    });
  }

  // Categorizar inspeções por status
  categorizeInspections(): void {
    const today = new Date();
    
    this.upcomingInspections = this.inspections.filter(i => {
      const inspDate = new Date(i.inspectionDate);
      return i.result === 'Pending' && inspDate > today;
    });

    this.completedInspections = this.inspections.filter(i => 
      i.result === 'Approved' || i.result === 'Rejected' || i.result === 'Conditional'
    );

    this.overdueInspections = this.inspections.filter(i => {
      const expiry = new Date(i.expiryDate);
      return expiry < today && i.result === 'Pending';
    });
  }


  getDaysUntilExpiry(expiryDate: Date): number {
    const today = new Date();
    const expiry = new Date(expiryDate);
    const diffTime = expiry.getTime() - today.getTime();
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  getDaysSinceExpiry(expiryDate: Date): number {
    return Math.abs(this.getDaysUntilExpiry(expiryDate));
  }

  // Traduzir InspectionType do enum para português
  getTypeLabel(type: string): string {
    const typeMap: { [key: string]: string } = {
      'Periodic': 'Inspeção Periódica',
      'Extraordinary': 'Inspeção Extraordinária',
      'Recheck': 'Reinspeção'
    };
    return typeMap[type] || type;
  }

  // Traduzir InspectionResult do enum para português
  getResultLabel(result: string): string {
    const resultMap: { [key: string]: string } = {
      'Pending': 'Pendente',
      'Approved': 'Aprovado',
      'Rejected': 'Reprovado',
      'Conditional': 'Condicional'
    };
    return resultMap[result] || result;
  }

  // Get result badge color class
  getResultBadgeClass(result: string): string {
    const colorMap: { [key: string]: string } = {
      'Pending': 'badge-yellow',
      'Approved': 'badge-green',
      'Rejected': 'badge-red',
      'Conditional': 'badge-orange'
    };
    return colorMap[result] || 'badge-gray';
  }

  // Get status badge color class (alias for getResultBadgeClass)
  getStatusBadgeClass(status: string): string {
    return this.getResultBadgeClass(status);
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-PT', {
      style: 'currency',
      currency: 'EUR'
    }).format(value);
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('pt-PT', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  isCustomer(): boolean {
    return this.authService.getCurrentUser()?.role === 'Customer';
  }
}


