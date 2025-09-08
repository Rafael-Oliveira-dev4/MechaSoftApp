import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceOrderModel } from '../core/models/service-order.model';

@Injectable({ providedIn: 'root' })
export class ServiceOrderService {
  private readonly baseUrl = '/api/service-orders';

  constructor(private readonly httpClient: HttpClient) {}

  // Stubs para integração futura
  getAll(): Observable<ServiceOrderModel[]> {
    return this.httpClient.get<ServiceOrderModel[]>(this.baseUrl);
  }
}


