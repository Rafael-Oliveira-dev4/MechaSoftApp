import { FuelType } from './enums';

export interface VehicleModel {
  id: string;
  customerId: string;
  brand: string;
  model: string;
  year: number;
  licensePlate: string;
  color: string;
  mileage?: number;
  chassisNumber?: string;
  engineNumber?: string;
  fuelType: FuelType;
  engineDisplacement?: number;
  enginePower?: number;
  inspectionExpiryDate?: string;
}


