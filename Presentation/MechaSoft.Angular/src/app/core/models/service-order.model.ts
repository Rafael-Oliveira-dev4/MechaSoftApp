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
  services?: ServiceItemDto[];
  parts?: PartItemDto[];
}

export interface ServiceItemDto {
  id: string;
  serviceId: string;
  serviceName: string;
  quantity: number;
  estimatedHours: number;
  unitPrice: number;
  discountPercentage?: number;
  totalPrice: number;
  status: string;
  mechanicId?: string;
  mechanicName?: string;
}

export interface PartItemDto {
  partId: string;
  partName: string;
  partCode: string;
  quantity: number;
  unitPrice: number;
  discountPercentage?: number;
  totalPrice: number;
}


