import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { LoginRequest, LoginResponse, RegisterRequest, User } from '../models/api.models';
import { Result, failure, success } from '../models/result.model';
import { ApiConfigService } from './api-config.service';

@Injectable({
  providedIn: 'root',
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
    return this.http.post<LoginResponse>(`${this.apiUrl}/accounts/login`, credentials).pipe(
      map(response => {
        this.setTokens(response.accessToken, response.refreshToken);

        // Build user object from login response
        const user: User = {
          id: response.userId,
          username: response.username,
          email: response.email,
          role: response.role,
          isActive: true,
          emailConfirmed: false,
          createdAt: new Date(),
        };

        this.currentUserSubject.next(user);
        return success(response);
      }),
      catchError(error => {
        return of(failure<LoginResponse>(error));
      })
    );
  }

  // Registar novo usuário
  register(userData: RegisterRequest): Observable<Result<void>> {
    return this.http.post<void>(`${this.apiUrl}/accounts/register`, userData).pipe(
      map(() => success(undefined)),
      catchError(error => of(failure<void>(error)))
    );
  }

  // Verificar disponibilidade de username
  checkUsernameAvailability(username: string): Observable<boolean> {
    return this.http
      .get<{ isAvailable: boolean }>(`${this.apiUrl}/accounts/check-username/${username}`)
      .pipe(
        map(response => response.isAvailable),
        catchError(() => of(true)) // Em caso de erro, assume disponível para não bloquear
      );
  }

  // Verificar disponibilidade de email
  checkEmailAvailability(email: string): Observable<boolean> {
    return this.http
      .get<{ isAvailable: boolean }>(`${this.apiUrl}/accounts/check-email/${email}`)
      .pipe(
        map(response => response.isAvailable),
        catchError(() => of(true)) // Em caso de erro, assume disponível para não bloquear
      );
  }

  // Sugerir usernames disponíveis
  suggestUsername(username: string): Observable<string[]> {
    return this.http
      .get<{ suggestions: string[] }>(`${this.apiUrl}/accounts/suggest-username/${username}`)
      .pipe(
        map(response => response.suggestions),
        catchError(() => of([])) // Em caso de erro, retorna array vazio
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
    const accessToken = this.getAccessToken();
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/accounts/refresh-token`, { accessToken, refreshToken })
      .pipe(
        map(response => {
          this.setTokens(response.accessToken, response.refreshToken);

          // Atualizar utilizador corrente a partir do novo access token
          const userFromToken = this.getUserFromToken(response.accessToken);
          if (userFromToken && this.isValidUser(userFromToken)) {
            this.currentUserSubject.next(userFromToken);
          }
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
      const user = this.getUserFromToken(token);
      if (user && this.isValidUser(user)) {
        this.currentUserSubject.next(user);
      } else {
        console.warn('Token inválido detectado. A limpar sessão...');
        this.logout();
      }
    } else if (token) {
      console.warn('Token expirado detectado. A limpar sessão...');
      this.logout();
    }
  }

  private isValidUser(user: User): boolean {
    return !!(
      user &&
      user.id &&
      user.username &&
      user.email &&
      user.role &&
      typeof user.username === 'string' &&
      typeof user.email === 'string' &&
      typeof user.role === 'string'
    );
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

  private getUserFromToken(token: string): User | null {
    try {
      const decoded = this.decodeToken(token);
      if (!decoded) return null;

      // Map JWT claims to User object
      // Claims from backend: nameid, sub, email, role
      const user: User = {
        id: String(
          decoded.nameid ||
            decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ||
            ''
        ),
        username: String(
          decoded.sub || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || ''
        ),
        email: String(
          decoded.email ||
            decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ||
            ''
        ),
        role: String(
          decoded.role ||
            decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
            ''
        ),
        isActive: true,
        emailConfirmed: false,
        createdAt: new Date(),
      };

      // Validate required fields
      if (!user.id || !user.username || !user.email || !user.role) {
        console.warn('Token claims incomplete, clearing session');
        return null;
      }

      return user;
    } catch (error) {
      console.error('Error parsing user from token, clearing session:', error);
      return null;
    }
  }

  private decodeToken(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      if (!base64Url) return null;

      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
          })
          .join('')
      );
      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }
}
