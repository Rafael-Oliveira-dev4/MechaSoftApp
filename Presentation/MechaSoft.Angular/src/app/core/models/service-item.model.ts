import { MoneyModel } from './common';
import { ServiceItemStatus } from './enums';

export interface ServiceItemModel {
  id: string;
  serviceOrderId: string;
  serviceId: string;
  quantity: number;
  estimatedHours: number;
  actualHours?: number;
  unitPrice: MoneyModel;
  discountPercentage?: number;
  totalPrice: MoneyModel;
  status: ServiceItemStatus;
  notes?: string;
  startedAt?: string;
  completedAt?: string;
  mechanicId?: string;
}


