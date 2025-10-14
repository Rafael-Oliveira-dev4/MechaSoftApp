/**
 * Currency Utilities - Formatação de moeda para Portugal
 */

/**
 * Formata valor para moeda portuguesa (EUR)
 * @param value Valor numérico
 * @param showSymbol Mostrar símbolo € (default: true)
 * @returns String formatada (ex: "1.234,56 €")
 */
export function formatCurrency(value: number, showSymbol: boolean = true): string {
  const formatted = new Intl.NumberFormat('pt-PT', {
    style: 'currency',
    currency: 'EUR',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  }).format(value);

  return showSymbol ? formatted : formatted.replace(/\s*€/, '');
}

/**
 * Formata valor sem símbolo de moeda
 * @param value Valor numérico
 * @returns String formatada (ex: "1.234,56")
 */
export function formatNumber(value: number, decimals: number = 2): string {
  return new Intl.NumberFormat('pt-PT', {
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals
  }).format(value);
}

/**
 * Parse string formatada PT para número
 * @param value String formatada (ex: "1.234,56" ou "1.234,56 €")
 * @returns Número ou null se inválido
 */
export function parseCurrency(value: string): number | null {
  if (!value) return null;
  
  // Remove espaços e símbolo €
  const cleaned = value.replace(/\s*€/g, '').trim();
  
  // Substitui . por nada e , por .
  const normalized = cleaned.replace(/\./g, '').replace(/,/, '.');
  
  const parsed = parseFloat(normalized);
  return isNaN(parsed) ? null : parsed;
}

/**
 * Formata percentagem
 * @param value Valor decimal (0.15 = 15%)
 * @returns String formatada (ex: "15%")
 */
export function formatPercentage(value: number, decimals: number = 0): string {
  return new Intl.NumberFormat('pt-PT', {
    style: 'percent',
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals
  }).format(value);
}

