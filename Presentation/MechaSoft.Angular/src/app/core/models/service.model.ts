export interface MechanicService {
  id: string;
  name: string;
  description: string;
  category: string;
  estimatedHours: number;
  pricePerHour?: number;
  fixedPrice?: number;
  requiresInspection: boolean;
  isActive: boolean;
  createdAt: Date;
  updatedAt?: Date;
}

export interface CreateServiceRequest {
  name: string;
  description: string;
  category: string;
  estimatedHours: number;
  pricePerHour?: number;
  fixedPrice?: number;
  requiresInspection: boolean;
}

export interface UpdateServiceRequest {
  id: string;
  name: string;
  description: string;
  category: string;
  estimatedHours: number;
  pricePerHour?: number;
  fixedPrice?: number;
  requiresInspection: boolean;
}

export interface ServicesResponse {
  items: MechanicService[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

