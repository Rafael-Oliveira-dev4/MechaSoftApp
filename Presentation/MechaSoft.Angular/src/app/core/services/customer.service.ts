import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { 
  Customer, 
  CreateCustomerRequest, 
  PaginationParams,
  PaginatedResponse 
} from '../models/api.models';
import { Result, success, failure, CommonErrors } from '../models/result.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = `${this.apiConfig.getApiUrl()}/customers`;
  }

  // Listar clientes com paginação
  getAll(params?: PaginationParams): Observable<Result<PaginatedResponse<Customer>>> {
    let httpParams = new HttpParams();
    
    if (params) {
      if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
      if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
      if (params.sortDescending !== undefined) httpParams = httpParams.set('sortDescending', params.sortDescending.toString());
    }

    return this.http.get<PaginatedResponse<Customer>>(this.apiUrl, { params: httpParams }).pipe(
      map(response => success(response)),
      catchError(error => of(failure<PaginatedResponse<Customer>>(error)))
    );
  }

  // Buscar cliente por ID
  getById(id: string): Observable<Result<Customer>> {
    return this.http.get<Customer>(`${this.apiUrl}/${id}`).pipe(
      map(customer => success(customer)),
      catchError(error => of(failure<Customer>(error || CommonErrors.NotFound('Cliente'))))
    );
  }

  // Criar novo cliente
  create(request: CreateCustomerRequest): Observable<Result<Customer>> {
    return this.http.post<Customer>(this.apiUrl, request).pipe(
      map(customer => success(customer)),
      catchError(error => of(failure<Customer>(error)))
    );
  }

  // Atualizar cliente existente
  update(id: string, request: CreateCustomerRequest): Observable<Result<Customer>> {
    return this.http.put<Customer>(`${this.apiUrl}/${id}`, request).pipe(
      map(customer => success(customer)),
      catchError(error => of(failure<Customer>(error)))
    );
  }

  // Soft-delete (ativar/desativar cliente)
  toggleActive(id: string): Observable<Result<void>> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/toggle-active`, {}).pipe(
      map(() => success(undefined)),
      catchError(error => of(failure<void>(error)))
    );
  }

  // Buscar clientes por termo de pesquisa
  search(searchTerm: string): Observable<Result<Customer[]>> {
    return this.getAll({ searchTerm, pageSize: 50 }).pipe(
      map(result => {
        if (result.isSuccess && result.value) {
          return success(result.value.items);
        }
        return failure(result.error || CommonErrors.ServerError());
      })
    );
  }
}
