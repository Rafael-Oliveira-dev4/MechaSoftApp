import { Pipe, PipeTransform } from '@angular/core';

/**
 * Pipe para formatação de telefone português
 * Usage: {{ phone | phonePt }}
 * Input: "912345678"
 * Output: "912 345 678"
 */
@Pipe({
  name: 'phonePt',
  standalone: true
})
export class PhonePtPipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) return '-';
    
    // Remove espaços existentes
    const cleaned = value.replace(/\s/g, '');
    
    // Valida se tem 9 dígitos
    if (cleaned.length !== 9) return value;
    
    // Formata: 912 345 678
    return `${cleaned.substring(0, 3)} ${cleaned.substring(3, 6)} ${cleaned.substring(6, 9)}`;
  }
}

