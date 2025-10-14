import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { Result, success, failure, CommonErrors } from '../models/result.model';
import { ServiceOrder, CreateServiceOrderRequest } from '../models/api.models';

// Request DTOs para adicionar services/parts a ordens
export interface AddServiceToOrderRequest {
  serviceId: string;
  quantity: number;
  estimatedHours: number;
  discountPercentage?: number;
  mechanicId?: string;
}

export interface AddPartToOrderRequest {
  partId: string;
  quantity: number;
  discountPercentage?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ServiceOrderService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = `${this.apiConfig.getApiUrl()}/service-orders`;
  }

  // Listar ordens de serviço com filtros
  getAll(
    pageNumber: number = 1,
    pageSize: number = 10,
    status?: string,
    customerId?: string
  ): Observable<Result<any>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (status) params = params.set('status', status);
    if (customerId) params = params.set('customerId', customerId);

    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(response => {
        // Transform backend response to match frontend interface
        const transformed = {
          items: response.serviceOrders || [],
          totalCount: response.totalCount || 0,
          pageNumber: response.pageNumber || 1,
          pageSize: response.pageSize || 10,
          totalPages: response.totalPages || 0
        };
        return success(transformed);
      }),
      catchError(error => of(failure<any>(error)))
    );
  }

  // Buscar ordem por ID
  getById(id: string): Observable<Result<ServiceOrder>> {
    return this.http.get<ServiceOrder>(`${this.apiUrl}/${id}`).pipe(
      map(order => success(order)),
      catchError(error => of(failure<ServiceOrder>(error || CommonErrors.NotFound('Ordem de Serviço'))))
    );
  }

  // Criar nova ordem de serviço
  create(request: CreateServiceOrderRequest): Observable<Result<ServiceOrder>> {
    return this.http.post<ServiceOrder>(this.apiUrl, request).pipe(
      map(order => success(order)),
      catchError(error => of(failure<ServiceOrder>(error)))
    );
  }

  // Atualizar estado da ordem
  updateStatus(id: string, status: string, notes?: string): Observable<Result<any>> {
    return this.http.put<any>(`${this.apiUrl}/${id}/status`, { status, notes: notes || null }).pipe(
      map(response => success(response)),
      catchError(error => of(failure<any>(error)))
    );
  }

  // Atribuir mecânico à ordem
  assignMechanic(orderId: string, mechanicId: string): Observable<Result<any>> {
    return this.http.put<any>(`${this.apiUrl}/${orderId}/mechanic`, { mechanicId }).pipe(
      map(response => success(response)),
      catchError(error => of(failure<any>(error)))
    );
  }

  // Adicionar serviço à ordem
  addService(orderId: string, request: AddServiceToOrderRequest): Observable<Result<any>> {
    return this.http.post<any>(`${this.apiUrl}/${orderId}/services`, request).pipe(
      map(response => success(response)),
      catchError(error => of(failure<any>(error)))
    );
  }

  // Adicionar peça à ordem
  addPart(orderId: string, request: AddPartToOrderRequest): Observable<Result<any>> {
    return this.http.post<any>(`${this.apiUrl}/${orderId}/parts`, request).pipe(
      map(response => success(response)),
      catchError(error => of(failure<any>(error)))
    );
  }

  // Cancelar ordem (devolver peças ao stock)
  cancel(id: string, reason: string): Observable<Result<void>> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/cancel`, { reason }).pipe(
      map(() => success(undefined)),
      catchError(error => of(failure<void>(error)))
    );
  }

  // Listar ordens de um cliente
  getByCustomer(customerId: string): Observable<Result<ServiceOrder[]>> {
    return this.http.get<ServiceOrder[]>(`${this.apiUrl}/customer/${customerId}`).pipe(
      map(orders => success(orders)),
      catchError(error => of(failure<ServiceOrder[]>(error)))
    );
  }
}
