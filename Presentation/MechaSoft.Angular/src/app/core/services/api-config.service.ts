import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiConfigService {
  private readonly baseUrl = 'https://localhost:7277/api'; // Backend HTTPS URL
  private readonly httpUrl = 'http://localhost:5039/api';  // Backend HTTP URL (fallback)

  constructor() { }

  getApiUrl(): string {
    // Por padrão, usar HTTPS. Se houver problemas, pode trocar para HTTP
    return this.baseUrl;
  }

  getHttpApiUrl(): string {
    return this.httpUrl;
  }
}
