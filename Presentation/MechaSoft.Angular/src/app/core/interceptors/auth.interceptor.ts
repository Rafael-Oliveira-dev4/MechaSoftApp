import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take, timeout } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(
    null
  );

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Get the auth token from the service
    const authToken = this.authService.getAccessToken();

    // Clone the request and add the authorization header if token exists
    if (authToken) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${authToken}`,
        },
      });
    }

    // Handle the request and catch errors
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          const refreshToken = this.authService.getRefreshToken();
          // Se não houver refresh token, sair
          if (!refreshToken) {
            this.authService.logout();
            return throwError(() => error);
          }

          // Evitar múltiplos refresh simultâneos
          if (this.isRefreshing) {
            return this.refreshTokenSubject.pipe(
              filter((token: string | null) => token !== null),
              take(1),
              switchMap((newToken: string | null) => {
                const cloned = request.clone({
                  setHeaders: { Authorization: `Bearer ${newToken}` },
                });
                return next.handle(cloned);
              })
            );
          } else {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);

            return this.authService.refreshAccessToken().pipe(
              timeout(10000),
              switchMap((result: any) => {
                this.isRefreshing = false;
                if (result.isSuccess && result.value?.accessToken) {
                  const newToken = result.value.accessToken;
                  this.refreshTokenSubject.next(newToken);
                  const cloned = request.clone({
                    setHeaders: { Authorization: `Bearer ${newToken}` },
                  });
                  return next.handle(cloned);
                }
                // Falhou refresh -> logout
                this.authService.logout();
                return throwError(() => error);
              }),
              catchError((refreshErr: unknown) => {
                this.isRefreshing = false;
                this.authService.logout();
                return throwError(() => refreshErr);
              })
            );
          }
        }
        return throwError(() => error);
      })
    );
  }
}
