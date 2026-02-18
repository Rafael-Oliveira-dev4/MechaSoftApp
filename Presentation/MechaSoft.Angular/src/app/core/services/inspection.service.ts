import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { Result, success, failure, CommonErrors } from '../models/result.model';

// Aligned with backend InspectionDto
export interface Inspection {
  id: string;
  vehicleId: string;
  vehicleInfo: string;
  type: string; // InspectionType: Periodic, Extraordinary, Recheck
  inspectionDate: Date;
  expiryDate: Date;
  result: string; // InspectionResult: Pending, Approved, Rejected, Conditional
  status: string; // Alias para result para compatibilidade
  cost: number;
  inspectionCenter: string;
  vehicleMileage: number;
  certificateNumber?: string;
  observations?: string;
  vehiclePlate: string; // Adicionado para compatibilidade
  scheduledDate: Date; // Alias para inspectionDate
  inspectorName: string; // Adicionado para compatibilidade
  inspectionType: string; // Alias para type
  completedDate: Date; // Adicionado para compatibilidade
}

export interface CreateInspectionRequest {
  vehicleId: string;
  serviceOrderId: string;
  type: string; // InspectionType enum
  inspectionDate: Date;
  expiryDate: Date;
  cost: number;
  inspectionCenter: string;
  vehicleMileage: number;
  observations?: string;
}

export interface UpdateInspectionResultRequest {
  result: string; // InspectionResult enum
  certificateNumber?: string;
  observations?: string;
}

@Injectable({
  providedIn: 'root'
})
export class InspectionService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = `${this.apiConfig.getApiUrl()}/inspections`;
  }

  // Listar inspeções com paginação
  getAll(pageNumber: number = 1, pageSize: number = 10, vehicleId?: string, result?: string): Observable<Result<any>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (vehicleId) params = params.set('vehicleId', vehicleId);
    if (result) params = params.set('result', result);

    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(response => {
        // Transform backend response to match frontend interface
        const transformed = {
          items: response.inspections || [],
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

  // Buscar inspeção por ID
  getById(id: string): Observable<Result<Inspection>> {
    return this.http.get<Inspection>(`${this.apiUrl}/${id}`).pipe(
      map(inspection => success(inspection)),
      catchError(error => of(failure<Inspection>(error || CommonErrors.NotFound('Inspeção'))))
    );
  }

  // Criar nova inspeção
  create(request: CreateInspectionRequest): Observable<Result<Inspection>> {
    return this.http.post<Inspection>(this.apiUrl, request).pipe(
      map(inspection => success(inspection)),
      catchError(error => of(failure<Inspection>(error)))
    );
  }

  // Atualizar resultado da inspeção
  updateResult(id: string, request: UpdateInspectionResultRequest): Observable<Result<void>> {
    return this.http.put<void>(`${this.apiUrl}/${id}/result`, request).pipe(
      map(() => success(undefined)),
      catchError(error => of(failure<void>(error)))
    );
  }
}
