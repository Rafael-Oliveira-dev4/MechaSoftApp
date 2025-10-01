import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ErrorDetail, CommonErrors } from '../models/result.model';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      retry({
        count: 2,
        delay: 1000,
        resetOnSuccess: true
      }),
      catchError((error: HttpErrorResponse) => {
        const errorDetail = this.handleError(error);
        
        console.error('HTTP Error:', {
          url: req.url,
          method: req.method,
          status: error.status,
          error: errorDetail
        });
        
        return throwError(() => errorDetail);
      })
    );
  }
  
  private handleError(error: HttpErrorResponse): ErrorDetail {
    if (error.error instanceof ErrorEvent) {
      return {
        code: 'NETWORK_ERROR',
        message: 'Erro de conexão. Verifique sua internet.',
        details: error.error.message,
        statusCode: 0
      };
    }
    
    switch (error.status) {
      case 400:
        return this.handle400Error(error);
      case 401:
        return CommonErrors.Unauthorized();
      case 404:
        return CommonErrors.NotFound('Recurso');
      case 409:
        return CommonErrors.Conflict(error.error?.message || 'Conflito');
      case 500:
      case 502:
      case 503:
        return CommonErrors.ServerError(error.error?.message);
      default:
        return {
          code: 'UNKNOWN_ERROR',
          message: error.error?.message || 'Erro desconhecido',
          details: error.message,
          statusCode: error.status
        };
    }
  }
  
  private handle400Error(error: HttpErrorResponse): ErrorDetail {
    const backendError = error.error;
    
    if (backendError?.errors) {
      const validationErrors = Object.entries(backendError.errors)
        .map(([field, errors]) => `${field}: ${(errors as string[]).join(', ')}`)
        .join('\n');
      
      return {
        code: 'VALIDATION_ERROR',
        message: backendError.message || 'Erro de validação',
        details: validationErrors,
        statusCode: 400
      };
    }
    
    return CommonErrors.ValidationFailed(backendError?.message || 'Dados inválidos');
  }
}
