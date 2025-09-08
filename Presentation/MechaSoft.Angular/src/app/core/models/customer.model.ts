import { AddressModel, NameModel } from './common';
import { CustomerType } from './enums';

export interface CustomerModel {
  id: string;
  name: NameModel;
  email: string;
  phone: string;
  nif?: string;
  citizenCard?: string;
  address: AddressModel;
  type: CustomerType;
  notes?: string;
}


