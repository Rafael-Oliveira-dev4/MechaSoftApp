import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validador de NIF (Número de Identificação Fiscal) Português
 * Um NIF válido deve ter 9 dígitos e passar na validação do algoritmo de verificação
 */
export function nifValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null; // Não validar se estiver vazio (use Validators.required para isso)
    }

    // Remove espaços e caracteres especiais
    const cleanNif = value.replace(/\s/g, '');

    // Verifica se tem exatamente 9 dígitos
    if (!/^\d{9}$/.test(cleanNif)) {
      return { invalidNif: { message: 'NIF deve ter exatamente 9 dígitos' } };
    }

    // Validação do algoritmo de verificação do NIF
    if (!validateNifChecksum(cleanNif)) {
      return { invalidNif: { message: 'NIF inválido' } };
    }

    return null;
  };
}

/**
 * Valida o checksum do NIF usando o algoritmo português
 */
function validateNifChecksum(nif: string): boolean {
  const digits = nif.split('').map(d => parseInt(d, 10));
  
  // Primeiro dígito deve ser 1, 2, 3, 5, 6, 8 ou 9
  const firstDigit = digits[0];
  if (![1, 2, 3, 5, 6, 8, 9].includes(firstDigit)) {
    return false;
  }

  // Calcular checksum
  let sum = 0;
  for (let i = 0; i < 8; i++) {
    sum += digits[i] * (9 - i);
  }

  const checkDigit = 11 - (sum % 11);
  const expectedCheckDigit = checkDigit >= 10 ? 0 : checkDigit;

  return digits[8] === expectedCheckDigit;
}

/**
 * Formata um NIF para exibição (XXX XXX XXX)
 */
export function formatNif(nif: string): string {
  if (!nif) return '';
  
  const cleanNif = nif.replace(/\s/g, '');
  
  if (cleanNif.length !== 9) return nif;
  
  return `${cleanNif.substring(0, 3)} ${cleanNif.substring(3, 6)} ${cleanNif.substring(6, 9)}`;
}

