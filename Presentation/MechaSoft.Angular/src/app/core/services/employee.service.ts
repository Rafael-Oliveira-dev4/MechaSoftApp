import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';
import { Result, success, failure, CommonErrors } from '../models/result.model';
import { 
  Employee, 
  CreateEmployeeRequest,
  UpdateEmployeeRequest,
  EmployeesResponse 
} from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = `${this.apiConfig.getApiUrl()}/employees`;
  }

  // Listar funcionários
  getAll(pageNumber: number = 1, pageSize: number = 10): Observable<Result<EmployeesResponse>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<EmployeesResponse>(this.apiUrl, { params }).pipe(
      map(response => success(response)),
      catchError(error => of(failure<EmployeesResponse>(error)))
    );
  }

  // Buscar funcionário por ID
  getById(id: string): Observable<Result<Employee>> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`).pipe(
      map(employee => success(employee)),
      catchError(error => of(failure<Employee>(error || CommonErrors.NotFound('Funcionário'))))
    );
  }

  // Criar novo funcionário
  create(request: CreateEmployeeRequest): Observable<Result<Employee>> {
    return this.http.post<Employee>(this.apiUrl, request).pipe(
      map(employee => success(employee)),
      catchError(error => of(failure<Employee>(error)))
    );
  }

  // Atualizar funcionário existente
  update(id: string, request: UpdateEmployeeRequest): Observable<Result<Employee>> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, { ...request, id }).pipe(
      map(employee => success(employee)),
      catchError(error => of(failure<Employee>(error)))
    );
  }

  // Listar apenas mecânicos
  getMechanics(): Observable<Result<Employee[]>> {
    return this.http.get<Employee[]>(`${this.apiUrl}/mechanics`).pipe(
      map(mechanics => success(mechanics)),
      catchError(error => of(failure<Employee[]>(error)))
    );
  }

  // Listar funcionários que podem fazer inspeções
  getInspectors(): Observable<Result<Employee[]>> {
    return this.getAll(1, 100).pipe(
      map(result => {
        if (result.isSuccess && result.value) {
          const inspectors = result.value.items.filter(e => e.canPerformInspections);
          return success(inspectors);
        }
        return failure<Employee[]>(result.error || CommonErrors.ServerError());
      })
    );
  }
}
