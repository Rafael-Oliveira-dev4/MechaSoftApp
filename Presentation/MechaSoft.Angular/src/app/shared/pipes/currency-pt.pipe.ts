import { Pipe, PipeTransform } from '@angular/core';
import { formatCurrency } from '../utils/currency.utils';

/**
 * Pipe para formatação de moeda portuguesa
 * Usage: {{ value | currencyPt }}
 * Output: "1.234,56 €"
 */
@Pipe({
  name: 'currencyPt',
  standalone: true
})
export class CurrencyPtPipe implements PipeTransform {
  transform(value: number | null | undefined, showSymbol: boolean = true): string {
    if (value === null || value === undefined) return '-';
    return formatCurrency(value, showSymbol);
  }
}

