import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfigService } from './api-config.service';
import { Vehicle, CreateVehicleRequest, PaginatedResponse } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {
    this.apiUrl = this.apiConfig.getApiUrl();
  }

  // Get all vehicles with pagination
  getVehicles(
    pageNumber: number = 1,
    pageSize: number = 10,
    customerId?: string,
    searchTerm?: string
  ): Observable<PaginatedResponse<Vehicle>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (customerId) {
      params = params.set('customerId', customerId);
    }

    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<PaginatedResponse<Vehicle>>(`${this.apiUrl}/vehicles`, { params });
  }

  // Get vehicle by ID
  getVehicleById(id: string): Observable<Vehicle> {
    return this.http.get<Vehicle>(`${this.apiUrl}/vehicles/${id}`);
  }

  // Get vehicles by customer ID
  getVehiclesByCustomer(customerId: string): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/vehicles/customer/${customerId}`);
  }

  // Create new vehicle
  createVehicle(vehicle: CreateVehicleRequest): Observable<Vehicle> {
    return this.http.post<Vehicle>(`${this.apiUrl}/vehicles`, vehicle);
  }

  // Update vehicle
  updateVehicle(id: string, vehicle: Partial<CreateVehicleRequest>): Observable<Vehicle> {
    return this.http.put<Vehicle>(`${this.apiUrl}/vehicles/${id}`, vehicle);
  }

  // Delete vehicle
  deleteVehicle(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/vehicles/${id}`);
  }

  // Search vehicles by license plate or model
  searchVehicles(searchTerm: string): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/vehicles/search`, {
      params: { searchTerm }
    });
  }
}
