export interface NameModel {
  firstName: string;
  lastName: string;
}

export interface AddressModel {
  street: string;
  number: string;
  parish: string;        // Freguesia
  municipality: string;  // Concelho
  district: string;      // Distrito
  postalCode: string;    // XXXX-XXX
  complement?: string;   // Complemento (andar, porta, etc.)
}

export interface MoneyModel {
  amount: number;
  currency?: string;
}


