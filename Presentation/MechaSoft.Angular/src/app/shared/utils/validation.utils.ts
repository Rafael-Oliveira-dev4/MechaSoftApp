/**
 * Validation Utilities - Validadores para Portugal
 */

/**
 * Valida NIF português
 * @param nif NIF a validar (9 dígitos)
 * @returns true se válido
 */
export function validateNIF(nif: string): boolean {
  if (!nif || nif.length !== 9) return false;
  
  const nifDigits = nif.split('').map(Number);
  const checkDigit = nifDigits[8];
  
  let sum = 0;
  for (let i = 0; i < 8; i++) {
    sum += nifDigits[i] * (9 - i);
  }
  
  const mod = sum % 11;
  const calculatedCheck = mod === 0 || mod === 1 ? 0 : 11 - mod;
  
  return calculatedCheck === checkDigit;
}

/**
 * Valida Cartão de Cidadão português
 * @param cc Cartão de Cidadão (12 dígitos + 2 letras + dígito de controlo)
 * @returns true se formato válido
 */
export function validateCitizenCard(cc: string): boolean {
  const pattern = /^[0-9]{8}\s?[0-9]\s?[A-Z]{2}[0-9]$/;
  return pattern.test(cc);
}

/**
 * Valida telefone português
 * @param phone Número de telefone
 * @returns true se válido (9 dígitos começando por 2 ou 9)
 */
export function validatePhone(phone: string): boolean {
  const cleaned = phone.replace(/\s/g, '');
  const pattern = /^[29][0-9]{8}$/;
  return pattern.test(cleaned);
}

/**
 * Valida código postal português
 * @param postalCode Código postal (xxxx-xxx)
 * @returns true se válido
 */
export function validatePostalCode(postalCode: string): boolean {
  const pattern = /^[0-9]{4}-[0-9]{3}$/;
  return pattern.test(postalCode);
}

/**
 * Valida email
 * @param email Email a validar
 * @returns true se válido
 */
export function validateEmail(email: string): boolean {
  const pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  return pattern.test(email);
}

/**
 * Valida matrícula portuguesa
 * @param plate Matrícula (XX-XX-XX ou XX-XX-XXX)
 * @returns true se válido
 */
export function validateLicensePlate(plate: string): boolean {
  const oldFormat = /^[0-9]{2}-[0-9]{2}-[A-Z]{2}$/;
  const newFormat = /^[0-9]{2}-[A-Z]{2}-[0-9]{2}$/;
  const newerFormat = /^[A-Z]{2}-[0-9]{2}-[A-Z]{2}$/;
  
  return oldFormat.test(plate) || newFormat.test(plate) || newerFormat.test(plate);
}

