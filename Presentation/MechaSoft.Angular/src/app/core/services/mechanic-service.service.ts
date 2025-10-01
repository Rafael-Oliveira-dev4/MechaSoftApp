import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { Result, success, failure, CommonErrors } from '../models/result.model';
import { 
  MechanicService, 
  CreateServiceRequest,
  UpdateServiceRequest,
  ServicesResponse 
} from '../models/service.model';

@Injectable({
  providedIn: 'root'
})
export class MechanicServiceService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = `${this.apiConfig.getApiUrl()}/services`;
  }

  // Listar serviços de oficina com paginação
  getAll(pageNumber: number = 1, pageSize: number = 10): Observable<Result<ServicesResponse>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<ServicesResponse>(this.apiUrl, { params }).pipe(
      map(response => success(response)),
      catchError(error => of(failure<ServicesResponse>(error)))
    );
  }

  // Buscar serviço por ID
  getById(id: string): Observable<Result<MechanicService>> {
    return this.http.get<MechanicService>(`${this.apiUrl}/${id}`).pipe(
      map(service => success(service)),
      catchError(error => of(failure<MechanicService>(error || CommonErrors.NotFound('Serviço'))))
    );
  }

  // Criar novo serviço
  create(request: CreateServiceRequest): Observable<Result<MechanicService>> {
    return this.http.post<MechanicService>(this.apiUrl, request).pipe(
      map(service => success(service)),
      catchError(error => of(failure<MechanicService>(error)))
    );
  }

  // Atualizar serviço existente
  update(id: string, request: UpdateServiceRequest): Observable<Result<MechanicService>> {
    return this.http.put<MechanicService>(`${this.apiUrl}/${id}`, { ...request, id }).pipe(
      map(service => success(service)),
      catchError(error => of(failure<MechanicService>(error)))
    );
  }

  // Listar serviços por categoria
  getByCategory(category: string): Observable<Result<MechanicService[]>> {
    const params = new HttpParams().set('category', category);
    return this.http.get<MechanicService[]>(this.apiUrl, { params }).pipe(
      map(services => success(services)),
      catchError(error => of(failure<MechanicService[]>(error)))
    );
  }

  // Listar apenas serviços ativos
  getActive(): Observable<Result<MechanicService[]>> {
    return this.getAll(1, 100).pipe(
      map(result => {
        if (result.isSuccess && result.value) {
          const active = result.value.items.filter(s => s.isActive);
          return success(active);
        }
        return failure<MechanicService[]>(result.error || CommonErrors.ServerError());
      })
    );
  }
}
