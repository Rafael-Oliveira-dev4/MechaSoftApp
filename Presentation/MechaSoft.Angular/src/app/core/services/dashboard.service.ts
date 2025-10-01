import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { Result, success, failure } from '../models/result.model';
import { DashboardStats, LowStockReport } from '../models/dashboard.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = `${this.apiConfig.getApiUrl()}/dashboard`;
  }

  // Obter estatísticas do dashboard
  getStats(): Observable<Result<DashboardStats>> {
    return this.http.get<DashboardStats>(`${this.apiUrl}/stats`).pipe(
      map(stats => success(stats)),
      catchError(error => of(failure<DashboardStats>(error)))
    );
  }

  // Obter relatório de peças com stock baixo
  getLowStockReport(): Observable<Result<LowStockReport>> {
    return this.http.get<LowStockReport>(`${this.apiUrl}/low-stock-report`).pipe(
      map(report => success(report)),
      catchError(error => of(failure<LowStockReport>(error)))
    );
  }

  // Obter faturamento por período
  getRevenue(period: 'today' | 'week' | 'month' | 'year'): Observable<Result<number>> {
    return this.http.get<{ revenue: number }>(`${this.apiUrl}/revenue`, {
      params: { period }
    }).pipe(
      map(response => success(response.revenue)),
      catchError(error => of(failure<number>(error)))
    );
  }

  // Obter últimas ordens criadas
  getRecentOrders(limit: number = 5): Observable<Result<any[]>> {
    return this.http.get<any[]>(`${this.apiUrl}/recent-orders`, {
      params: { limit: limit.toString() }
    }).pipe(
      map(orders => success(orders)),
      catchError(error => of(failure<any[]>(error)))
    );
  }
}
