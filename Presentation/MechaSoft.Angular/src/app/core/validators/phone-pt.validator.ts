import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validador de Telefone Português
 * Aceita:
 * - Números fixos: 9 dígitos começando com 2
 * - Números móveis: 9 dígitos começando com 9
 * - Com ou sem espaços, hífens ou parênteses
 */
export function phonePortugueseValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null; // Não validar se estiver vazio (use Validators.required para isso)
    }

    // Remove espaços, hífens, parênteses e outros caracteres especiais
    const cleanPhone = value.replace(/[\s\-()]/g, '');

    // Verifica se tem exatamente 9 dígitos
    if (!/^\d{9}$/.test(cleanPhone)) {
      return { 
        invalidPhone: { 
          message: 'Telefone deve ter 9 dígitos' 
        } 
      };
    }

    // Valida se é um número português válido
    const firstDigit = cleanPhone[0];
    
    // Números fixos começam com 2
    // Números móveis começam com 9
    if (!['2', '9'].includes(firstDigit)) {
      return { 
        invalidPhone: { 
          message: 'Telefone português deve começar com 2 (fixo) ou 9 (móvel)' 
        } 
      };
    }

    return null;
  };
}

/**
 * Validador de Telemóvel Português (apenas móveis)
 * Aceita apenas números que começam com 9
 */
export function mobilePhonePortugueseValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null;
    }

    const cleanPhone = value.replace(/[\s\-()]/g, '');

    if (!/^\d{9}$/.test(cleanPhone)) {
      return { 
        invalidMobile: { 
          message: 'Telemóvel deve ter 9 dígitos' 
        } 
      };
    }

    if (!cleanPhone.startsWith('9')) {
      return { 
        invalidMobile: { 
          message: 'Telemóvel português deve começar com 9' 
        } 
      };
    }

    return null;
  };
}

/**
 * Formata um telefone português para exibição
 * Exemplos:
 * - 912345678 -> 912 345 678 (móvel)
 * - 212345678 -> 21 234 5678 (fixo)
 */
export function formatPhonePortuguese(phone: string): string {
  if (!phone) return '';
  
  const cleanPhone = phone.replace(/[\s\-()]/g, '');
  
  if (cleanPhone.length !== 9) return phone;

  // Se começa com 9 (móvel): XXX XXX XXX
  if (cleanPhone.startsWith('9')) {
    return `${cleanPhone.substring(0, 3)} ${cleanPhone.substring(3, 6)} ${cleanPhone.substring(6, 9)}`;
  }
  
  // Se começa com 2 (fixo): XX XXX XXXX
  if (cleanPhone.startsWith('2')) {
    return `${cleanPhone.substring(0, 2)} ${cleanPhone.substring(2, 5)} ${cleanPhone.substring(5, 9)}`;
  }

  return phone;
}

