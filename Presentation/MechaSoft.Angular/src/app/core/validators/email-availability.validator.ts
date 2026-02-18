import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { catchError, debounceTime, first, map, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

export function emailAvailabilityValidator(authService: AuthService): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    if (!control.value) {
      return of(null);
    }

    return of(control.value).pipe(
      debounceTime(500),
      switchMap(email =>
        authService.checkEmailAvailability(email).pipe(
          map(isAvailable => (isAvailable ? null : { emailTaken: true })),
          catchError(() => of(null))
        )
      ),
      first()
    );
  };
}
