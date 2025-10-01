export interface ErrorDetail {
  code: string;
  message: string;
  details?: string;
  statusCode?: number;
}

export interface Result<T> {
  isSuccess: boolean;
  value?: T;
  error?: ErrorDetail;
}

export function success<T>(value: T): Result<T> {
  return {
    isSuccess: true,
    value
  };
}

export function failure<T>(error: ErrorDetail): Result<T> {
  return {
    isSuccess: false,
    error
  };
}

export const CommonErrors = {
  NotFound: (entity: string): ErrorDetail => ({
    code: 'NOT_FOUND',
    message: `${entity} não encontrado`,
    statusCode: 404
  }),
  
  ValidationFailed: (message: string): ErrorDetail => ({
    code: 'VALIDATION_FAILED',
    message,
    statusCode: 400
  }),
  
  Unauthorized: (): ErrorDetail => ({
    code: 'UNAUTHORIZED',
    message: 'Não autorizado',
    statusCode: 401
  }),
  
  ServerError: (message?: string): ErrorDetail => ({
    code: 'SERVER_ERROR',
    message: message || 'Erro interno do servidor',
    statusCode: 500
  }),
  
  Conflict: (message: string): ErrorDetail => ({
    code: 'CONFLICT',
    message,
    statusCode: 409
  })
};

