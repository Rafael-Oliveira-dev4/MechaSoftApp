import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { catchError, debounceTime, first, map, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

export function usernameAvailabilityExcludeValidator(
  authService: AuthService,
  currentUsername: string
): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    if (!control.value) {
      return of(null);
    }

    const username = control.value;

    // Se o username não mudou, não valida
    if (username === currentUsername) {
      return of(null);
    }

    // Validação básica antes de fazer a chamada HTTP
    if (username.length < 3) {
      return of(null); // Deixa a validação síncrona cuidar disso
    }

    return of(username).pipe(
      debounceTime(500), // Aguarda 500ms após o utilizador parar de digitar
      switchMap(usernameValue =>
        authService.checkUsernameAvailability(usernameValue).pipe(
          map(isAvailable => (isAvailable ? null : { usernameTaken: true })),
          catchError(() => of(null)) // Em caso de erro, não bloqueia
        )
      ),
      first()
    );
  };
}

