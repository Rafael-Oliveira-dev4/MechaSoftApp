import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VehicleService } from '../../../../services/vehicle.service';
import { Vehicle, PaginatedResponse } from '../../../../core/models/api.models';

@Component({
  selector: 'app-vehicles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.scss']
})
export class VehiclesComponent implements OnInit {
  vehicles: Vehicle[] = [];
  loading = false;
  error: string | null = null;
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  searchTerm = '';
  Math = Math; // Para usar Math no template

  private vehicleService = inject(VehicleService);

  ngOnInit() {
    this.loadVehicles();
  }

  loadVehicles() {
    this.loading = true;
    this.error = null;

    this.vehicleService.getAll(this.currentPage, this.pageSize, undefined, this.searchTerm)
      .subscribe({
        next: (response: PaginatedResponse<Vehicle>) => {
          this.vehicles = response.items;
          this.totalCount = response.totalCount;
          this.totalPages = response.totalPages;
          this.loading = false;
        },
        error: (err: any) => {
          console.error('Error loading vehicles:', err);
          this.error = 'Erro ao carregar veículos. Verifique se o backend está rodando.';
          this.loading = false;
        }
      });
  }

  onSearch() {
    this.currentPage = 1;
    this.loadVehicles();
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadVehicles();
  }

  clearSearch() {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadVehicles();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);
    
    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    
    return pages;
  }
}


