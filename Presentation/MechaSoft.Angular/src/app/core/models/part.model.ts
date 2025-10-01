export interface Part {
  id: string;
  partNumber: string;
  name: string;
  description: string;
  brand: string;
  category: string;
  unitPrice: number;
  quantityInStock: number;
  minimumStock: number;
  location: string;
  supplier?: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt?: Date;
}

export interface CreatePartRequest {
  partNumber: string;
  name: string;
  description: string;
  brand: string;
  category: string;
  unitPrice: number;
  quantityInStock: number;
  minimumStock: number;
  location: string;
  supplier?: string;
}

export interface UpdatePartRequest {
  id: string;
  partNumber: string;
  name: string;
  description: string;
  brand: string;
  category: string;
  unitPrice: number;
  location: string;
  supplier?: string;
}

export interface UpdateStockRequest {
  id: string;
  quantity: number;
  operation: 'add' | 'remove';
}

export interface PartsResponse {
  items: Part[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface LowStockPart {
  id: string;
  partNumber: string;
  name: string;
  quantityInStock: number;
  minimumStock: number;
  deficit: number;
}

