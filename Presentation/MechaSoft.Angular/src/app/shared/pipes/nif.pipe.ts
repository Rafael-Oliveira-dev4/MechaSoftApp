import { Pipe, PipeTransform } from '@angular/core';

/**
 * Pipe para formatação de NIF português
 * Usage: {{ nif | nifPt }}
 * Input: "123456789"
 * Output: "123 456 789"
 */
@Pipe({
  name: 'nifPt',
  standalone: true
})
export class NifPtPipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) return '-';
    
    // Remove espaços existentes
    const cleaned = value.replace(/\s/g, '');
    
    // Valida se tem 9 dígitos
    if (cleaned.length !== 9) return value;
    
    // Formata: 123 456 789
    return `${cleaned.substring(0, 3)} ${cleaned.substring(3, 6)} ${cleaned.substring(6, 9)}`;
  }
}

