import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { catchError, debounceTime, first, map, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

export function emailAvailabilityExcludeValidator(
  authService: AuthService,
  currentEmail: string
): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    if (!control.value) {
      return of(null);
    }

    const email = control.value;

    // Se o email não mudou, não valida
    if (email === currentEmail) {
      return of(null);
    }

    // Validação básica de email antes de fazer a chamada HTTP
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(email)) {
      return of(null); // Deixa a validação síncrona cuidar disso
    }

    return of(email).pipe(
      debounceTime(500), // Aguarda 500ms após o utilizador parar de digitar
      switchMap(emailValue =>
        authService.checkEmailAvailability(emailValue).pipe(
          map(isAvailable => (isAvailable ? null : { emailTaken: true })),
          catchError(() => of(null)) // Em caso de erro, não bloqueia
        )
      ),
      first()
    );
  };
}

