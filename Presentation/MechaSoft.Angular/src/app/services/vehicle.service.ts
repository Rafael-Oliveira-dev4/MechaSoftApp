import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VehicleModel } from '../core/models/vehicle.model';

@Injectable({ providedIn: 'root' })
export class VehicleService {
  private readonly baseUrl = '/api/vehicles';

  constructor(private readonly httpClient: HttpClient) {}

  // Stubs para integração futura
  getAll(): Observable<VehicleModel[]> {
    return this.httpClient.get<VehicleModel[]>(this.baseUrl);
  }
}


