import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { ApiConfigService } from './api-config.service';
import { LoginRequest, LoginResponse, RegisterRequest, User } from '../models/api.models';
import { Result, success, failure } from '../models/result.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl: string;
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.apiUrl = this.apiConfig.getApiUrl();
    if (isPlatformBrowser(this.platformId)) {
      this.loadUserFromStorage();
    }
  }

  // Autenticar usuário
  login(credentials: LoginRequest): Observable<Result<LoginResponse>> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/accounts/login`, credentials)
      .pipe(
        map(response => {
          this.setTokens(response.token, response.refreshToken);
          this.currentUserSubject.next(response.user);
          return success(response);
        }),
        catchError(error => of(failure<LoginResponse>(error)))
      );
  }

  // Registar novo usuário
  register(userData: RegisterRequest): Observable<Result<void>> {
    return this.http.post<void>(`${this.apiUrl}/accounts/register`, userData)
      .pipe(
        map(() => success(undefined)),
        catchError(error => of(failure<void>(error)))
      );
  }

  // Terminar sessão
  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('access_token');
      localStorage.removeItem('refresh_token');
    }
    this.currentUserSubject.next(null);
  }

  // Obter usuário atual
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  // Verificar se está autenticado
  isAuthenticated(): boolean {
    const token = this.getAccessToken();
    return !!token && !this.isTokenExpired(token);
  }

  // Obter access token
  getAccessToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('access_token');
    }
    return null;
  }

  // Obter refresh token
  getRefreshToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('refresh_token');
    }
    return null;
  }

  // Renovar access token
  refreshAccessToken(): Observable<Result<LoginResponse>> {
    const refreshToken = this.getRefreshToken();
    return this.http.post<LoginResponse>(`${this.apiUrl}/accounts/refresh-token`, { refreshToken })
      .pipe(
        map(response => {
          this.setTokens(response.token, response.refreshToken);
          this.currentUserSubject.next(response.user);
          return success(response);
        }),
        catchError(error => of(failure<LoginResponse>(error)))
      );
  }

  private setTokens(accessToken: string, refreshToken: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('access_token', accessToken);
      localStorage.setItem('refresh_token', refreshToken);
    }
  }

  private loadUserFromStorage(): void {
    const token = this.getAccessToken();
    if (token && !this.isTokenExpired(token)) {
      const userInfo = this.decodeToken(token);
      if (userInfo) {
        this.currentUserSubject.next(userInfo);
      }
    }
  }

  private isTokenExpired(token: string): boolean {
    try {
      const decoded = this.decodeToken(token);
      if (!decoded || !decoded.exp) return true;
      
      const expirationDate = new Date(decoded.exp * 1000);
      return expirationDate < new Date();
    } catch {
      return true;
    }
  }

  private decodeToken(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));
      return JSON.parse(jsonPayload);
    } catch {
      return null;
    }
  }
}
