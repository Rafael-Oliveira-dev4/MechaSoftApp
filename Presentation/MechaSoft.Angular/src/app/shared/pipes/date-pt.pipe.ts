import { Pipe, PipeTransform } from '@angular/core';
import { formatDate } from '../utils/date.utils';

/**
 * Pipe para formatação de data portuguesa
 * Usage: {{ date | datePt:'short' }}
 * Output: "13/10/2025"
 */
@Pipe({
  name: 'datePt',
  standalone: true
})
export class DatePtPipe implements PipeTransform {
  transform(value: Date | string | null, format: 'short' | 'medium' | 'long' | 'full' = 'short'): string {
    return formatDate(value, format);
  }
}

