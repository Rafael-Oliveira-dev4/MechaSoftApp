/**
 * Date Utilities - Formatação de datas para Portugal
 */

/**
 * Formata data para formato PT
 * @param date Data a formatar
 * @param format Formato (short, medium, long, full)
 * @returns String formatada
 */
export function formatDate(date: Date | string | null, format: 'short' | 'medium' | 'long' | 'full' = 'short'): string {
  if (!date) return '-';
  
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  
  const formats: Record<string, Intl.DateTimeFormatOptions> = {
    short: { day: '2-digit', month: '2-digit', year: 'numeric' }, // 13/10/2025
    medium: { day: '2-digit', month: 'short', year: 'numeric' }, // 13 out 2025
    long: { day: '2-digit', month: 'long', year: 'numeric' }, // 13 de outubro de 2025
    full: { weekday: 'long', day: '2-digit', month: 'long', year: 'numeric' } // Segunda-feira, 13 de outubro de 2025
  };

  return new Intl.DateTimeFormat('pt-PT', formats[format]).format(dateObj);
}

/**
 * Formata data e hora
 * @param date Data a formatar
 * @returns String formatada (ex: "13/10/2025 às 14:30")
 */
export function formatDateTime(date: Date | string | null): string {
  if (!date) return '-';
  
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  
  const datePart = formatDate(dateObj, 'short');
  const timePart = new Intl.DateTimeFormat('pt-PT', {
    hour: '2-digit',
    minute: '2-digit'
  }).format(dateObj);

  return `${datePart} às ${timePart}`;
}

/**
 * Calcula diferença de dias entre datas
 * @param date1 Primeira data
 * @param date2 Segunda data (default: hoje)
 * @returns Número de dias
 */
export function daysDifference(date1: Date | string, date2: Date | string = new Date()): number {
  const d1 = typeof date1 === 'string' ? new Date(date1) : date1;
  const d2 = typeof date2 === 'string' ? new Date(date2) : date2;
  
  const diffTime = Math.abs(d2.getTime() - d1.getTime());
  return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
}

/**
 * Verifica se data é passada
 * @param date Data a verificar
 * @returns true se a data já passou
 */
export function isPast(date: Date | string): boolean {
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  return dateObj < new Date();
}

/**
 * Formata data relativa (ex: "há 2 dias", "daqui a 3 dias")
 * @param date Data a formatar
 * @returns String formatada
 */
export function formatRelativeTime(date: Date | string): string {
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  const now = new Date();
  const diffMs = dateObj.getTime() - now.getTime();
  const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));
  
  if (diffDays === 0) return 'Hoje';
  if (diffDays === 1) return 'Amanhã';
  if (diffDays === -1) return 'Ontem';
  if (diffDays > 0) return `Daqui a ${diffDays} dias`;
  return `Há ${Math.abs(diffDays)} dias`;
}

