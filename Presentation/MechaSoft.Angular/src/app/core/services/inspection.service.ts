import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { Result, success, failure, CommonErrors } from '../models/result.model';

export interface Inspection {
  id: string;
  vehicleId: string;
  inspectorId: string;
  scheduledDate: Date;
  actualDate?: Date;
  result?: string;
  notes?: string;
  status: string;
}

export interface CreateInspectionRequest {
  vehicleId: string;
  inspectorId: string;
  scheduledDate: Date;
  notes?: string;
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

  // Listar inspeções
  getAll(pageNumber: number = 1, pageSize: number = 10, status?: string): Observable<Result<any>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (status) params = params.set('status', status);

    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(response => success(response)),
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
  updateResult(id: string, result: string, notes: string): Observable<Result<void>> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/result`, { result, notes }).pipe(
      map(() => success(undefined)),
      catchError(error => of(failure<void>(error)))
    );
  }

  // Listar inspeções de um veículo
  getByVehicle(vehicleId: string): Observable<Result<Inspection[]>> {
    return this.http.get<Inspection[]>(`${this.apiUrl}/vehicle/${vehicleId}`).pipe(
      map(inspections => success(inspections)),
      catchError(error => of(failure<Inspection[]>(error)))
    );
  }
}
