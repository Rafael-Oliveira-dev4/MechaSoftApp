import { MoneyModel } from './common';

export interface InspectionModel {
  id: string;
  serviceOrderId?: string;
  vehicleId: string;
  date?: string;
  result?: string;
  cost: MoneyModel;
}


