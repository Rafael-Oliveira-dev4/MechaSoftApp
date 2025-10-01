import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfigService } from './api-config.service';
import { Customer, CreateCustomerRequest, PaginatedResponse } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = this.apiConfig.getApiUrl();
  }

  // Get all customers with pagination
  getCustomers(
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string
  ): Observable<PaginatedResponse<Customer>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<PaginatedResponse<Customer>>(`${this.apiUrl}/customers`, { params });
  }

  // Get customer by ID
  getCustomerById(id: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiUrl}/customers/${id}`);
  }

  // Create new customer
  createCustomer(customer: CreateCustomerRequest): Observable<Customer> {
    return this.http.post<Customer>(`${this.apiUrl}/customers`, customer);
  }

  // Update customer
  updateCustomer(id: string, customer: Partial<CreateCustomerRequest>): Observable<Customer> {
    return this.http.put<Customer>(`${this.apiUrl}/customers/${id}`, customer);
  }

  // Delete customer
  deleteCustomer(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/customers/${id}`);
  }

  // Search customers by name or email
  searchCustomers(searchTerm: string): Observable<Customer[]> {
    return this.http.get<Customer[]>(`${this.apiUrl}/customers/search`, {
      params: { searchTerm }
    });
  }
}
