import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { 
  Vehicle, 
  CreateVehicleRequest,
  PaginationParams,
  PaginatedResponse 
} from '../models/api.models';
import { Result, success, failure, CommonErrors } from '../models/result.model';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = `${this.apiConfig.getApiUrl()}/vehicles`;
  }

  // Listar veículos com paginação
  getAll(params?: PaginationParams): Observable<Result<PaginatedResponse<Vehicle>>> {
    let httpParams = new HttpParams();
    
    if (params) {
      if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    }

    return this.http.get<PaginatedResponse<Vehicle>>(this.apiUrl, { params: httpParams }).pipe(
      map(response => success(response)),
      catchError(error => of(failure<PaginatedResponse<Vehicle>>(error)))
    );
  }

  // Buscar veículo por ID
  getById(id: string): Observable<Result<Vehicle>> {
    return this.http.get<Vehicle>(`${this.apiUrl}/${id}`).pipe(
      map(vehicle => success(vehicle)),
      catchError(error => of(failure<Vehicle>(error || CommonErrors.NotFound('Veículo'))))
    );
  }

  // Listar veículos de um cliente
  getByCustomer(customerId: string): Observable<Result<Vehicle[]>> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/customer/${customerId}`).pipe(
      map(vehicles => success(vehicles)),
      catchError(error => of(failure<Vehicle[]>(error)))
    );
  }

  // Buscar veículo por matrícula
  getByLicensePlate(licensePlate: string): Observable<Result<Vehicle>> {
    return this.http.get<Vehicle>(`${this.apiUrl}/plate/${licensePlate}`).pipe(
      map(vehicle => success(vehicle)),
      catchError(error => of(failure<Vehicle>(error || CommonErrors.NotFound('Veículo'))))
    );
  }

  // Criar novo veículo
  create(request: CreateVehicleRequest): Observable<Result<Vehicle>> {
    return this.http.post<Vehicle>(this.apiUrl, request).pipe(
      map(vehicle => success(vehicle)),
      catchError(error => of(failure<Vehicle>(error)))
    );
  }

  // Atualizar veículo existente
  update(id: string, request: CreateVehicleRequest): Observable<Result<Vehicle>> {
    return this.http.put<Vehicle>(`${this.apiUrl}/${id}`, request).pipe(
      map(vehicle => success(vehicle)),
      catchError(error => of(failure<Vehicle>(error)))
    );
  }

  // Listar veículos com inspeção vencida
  getExpiredInspections(): Observable<Result<Vehicle[]>> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/expired-inspections`).pipe(
      map(vehicles => success(vehicles)),
      catchError(error => of(failure<Vehicle[]>(error)))
    );
  }
}
