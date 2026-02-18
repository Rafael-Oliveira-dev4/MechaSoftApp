import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CommonErrors, ErrorDetail } from '../models/result.model';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        const errorDetail = this.handleError(error);

        console.error('HTTP Error:', {
          url: req.url,
          method: req.method,
          status: error.status,
          error: errorDetail,
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
        statusCode: 0,
      };
    }

    // status 0 = conexão recusada / servidor inacessível (API não está a correr ou CORS)
    if (error.status === 0) {
      return {
        code: 'CONNECTION_REFUSED',
        message: 'Não foi possível ligar ao servidor. Verifique se a API está a correr (ex.: http://localhost:5039).',
        details: error.message || 'Connection refused',
        statusCode: 0,
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
        // API devolve { code, description } em erros 5xx
        const serverMsg = error.error?.description ?? error.error?.message;
        return CommonErrors.ServerError(serverMsg);
      default:
        return {
          code: 'UNKNOWN_ERROR',
          message: error.error?.message || 'Erro desconhecido',
          details: error.message,
          statusCode: error.status,
        };
    }
  }

  private handle400Error(error: HttpErrorResponse): ErrorDetail {
    const backendError = error.error;

    // Log completo para debug
    console.error('400 Error - Full backend response:', backendError);

    // Check for backend error pattern: { code: "...", description: "..." }
    if (backendError?.code && backendError?.description) {
      return {
        code: backendError.code,
        message: this.translateErrorMessage(backendError.code, backendError.description),
        details: backendError.description,
        statusCode: 400,
      };
    }

    // Check for ASP.NET validation errors
    if (backendError?.errors) {
      const validationErrors = Object.entries(backendError.errors)
        .map(([field, errors]) => `${field}: ${(errors as string[]).join(', ')}`)
        .join('\n');

      return {
        code: 'VALIDATION_ERROR',
        message: backendError.message || 'Erro de validação',
        details: validationErrors,
        statusCode: 400,
      };
    }

    // Check for error.code and error.message from backend Result<T> pattern
    if (backendError?.code && backendError?.message) {
      return {
        code: backendError.code,
        message: backendError.message,
        details: backendError.details || JSON.stringify(backendError),
        statusCode: 400,
      };
    }

    return CommonErrors.ValidationFailed(backendError?.message || 'Dados inválidos');
  }

  private translateErrorMessage(code: string, fallback: string): string {
    const translations: Record<string, string> = {
      UsernameAlreadyExists: 'Este nome de utilizador já está em uso',
      EmailAlreadyExists: 'Este email já está registado',
      InvalidCredentials:
        'Username ou password incorretos. Por favor, verifique os seus dados e tente novamente.',
      UserNotFound: 'Utilizador não encontrado. Verifique o username ou email inserido.',
      AccountInactive: 'A sua conta está inativa. Contacte o suporte para mais informações.',
      UserLockedOut:
        'A sua conta foi temporariamente bloqueada por motivos de segurança. Tente novamente mais tarde.',
      PasswordExpired: 'A sua password expirou. Por favor, redefina a sua password.',
      InvalidToken: 'Sessão expirada. Por favor, faça login novamente.',
      UnauthorizedAccess: 'Não tem permissão para aceder a este recurso.',
    };

    return translations[code] || fallback;
  }
}
