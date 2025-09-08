import { MoneyModel } from './common';
import { Priority, ServiceOrderStatus } from './enums';

export interface ServiceOrderModel {
  id: string;
  orderNumber: string;
  customerId: string;
  vehicleId: string;
  description: string;
  status: ServiceOrderStatus;
  priority: Priority;
  estimatedCost: MoneyModel;
  finalCost?: MoneyModel;
  estimatedDelivery?: string;
  actualDelivery?: string;
  mechanicId?: string;
  actualHours?: number;
  requiresInspection: boolean;
  internalNotes?: string;
}


