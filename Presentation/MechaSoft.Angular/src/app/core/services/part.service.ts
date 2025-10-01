import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { Result, success, failure, CommonErrors } from '../models/result.model';
import { 
  Part, 
  CreatePartRequest,
  UpdatePartRequest,
  UpdateStockRequest,
  PartsResponse,
  LowStockPart 
} from '../models/part.model';

@Injectable({
  providedIn: 'root'
})
export class PartService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = `${this.apiConfig.getApiUrl()}/parts`;
  }

  // Listar peças com paginação
  getAll(pageNumber: number = 1, pageSize: number = 10, lowStock?: boolean): Observable<Result<PartsResponse>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (lowStock !== undefined) {
      params = params.set('lowStock', lowStock.toString());
    }

    return this.http.get<PartsResponse>(this.apiUrl, { params }).pipe(
      map(response => success(response)),
      catchError(error => of(failure<PartsResponse>(error)))
    );
  }

  // Buscar peça por ID
  getById(id: string): Observable<Result<Part>> {
    return this.http.get<Part>(`${this.apiUrl}/${id}`).pipe(
      map(part => success(part)),
      catchError(error => of(failure<Part>(error || CommonErrors.NotFound('Peça'))))
    );
  }

  // Criar nova peça
  create(request: CreatePartRequest): Observable<Result<Part>> {
    return this.http.post<Part>(this.apiUrl, request).pipe(
      map(part => success(part)),
      catchError(error => of(failure<Part>(error)))
    );
  }

  // Atualizar peça existente
  update(id: string, request: UpdatePartRequest): Observable<Result<Part>> {
    return this.http.put<Part>(`${this.apiUrl}/${id}`, { ...request, id }).pipe(
      map(part => success(part)),
      catchError(error => of(failure<Part>(error)))
    );
  }

  // Atualizar stock (adicionar ou remover)
  updateStock(id: string, quantity: number, operation: 'add' | 'remove'): Observable<Result<Part>> {
    const request: UpdateStockRequest = { id, quantity, operation };
    return this.http.put<Part>(`${this.apiUrl}/${id}/stock`, request).pipe(
      map(part => success(part)),
      catchError(error => of(failure<Part>(error)))
    );
  }

  // Soft-delete (ativar/desativar peça)
  toggleActive(id: string): Observable<Result<void>> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/toggle-active`, {}).pipe(
      map(() => success(undefined)),
      catchError(error => of(failure<void>(error)))
    );
  }

  // Listar peças com stock baixo
  getLowStock(): Observable<Result<LowStockPart[]>> {
    return this.http.get<LowStockPart[]>(`${this.apiUrl}/low-stock`).pipe(
      map(parts => success(parts)),
      catchError(error => of(failure<LowStockPart[]>(error)))
    );
  }

  // Buscar peças por termo
  search(searchTerm: string): Observable<Result<Part[]>> {
    const params = new HttpParams().set('searchTerm', searchTerm);
    return this.http.get<Part[]>(`${this.apiUrl}/search`, { params }).pipe(
      map(parts => success(parts)),
      catchError(error => of(failure<Part[]>(error)))
    );
  }
}
