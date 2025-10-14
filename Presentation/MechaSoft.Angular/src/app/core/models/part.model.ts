export interface Part {
  id: string;
  code: string; // API retorna 'code' não 'partNumber'
  name: string;
  description: string;
  brand: string;
  category: string;
  unitCost: number; // Custo unitário
  salePrice: number; // Preço de venda
  stockQuantity: number; // API retorna 'stockQuantity' não 'quantityInStock'
  minStockLevel: number; // API retorna 'minStockLevel' não 'minimumStock'
  isLowStock: boolean; // Flag calculada pela API
  location: string;
  supplierName?: string; // API retorna 'supplierName' não 'supplier'
  isActive: boolean;
  createdAt?: Date;
  updatedAt?: Date;
  
  // Legacy properties for backwards compatibility
  partNumber?: string;
  unitPrice?: number;
  quantityInStock?: number;
  minimumStock?: number;
  supplier?: string;
}

export interface CreatePartRequest {
  code: string; // Backend espera 'code' não 'partNumber'
  name: string;
  description: string;
  brand: string;
  category: string;
  unitCost: number; // Custo unitário
  salePrice: number; // Preço de venda
  stockQuantity: number; // Backend espera 'stockQuantity'
  minStockLevel: number; // Backend espera 'minStockLevel'
  location: string;
  supplierName?: string; // Backend espera 'supplierName'
}

export interface UpdatePartRequest {
  id: string;
  code: string; // Backend espera 'code' não 'partNumber'
  name: string;
  description: string;
  brand: string;
  category: string;
  unitCost: number; // Custo unitário
  salePrice: number; // Preço de venda
  location: string;
  supplierName?: string; // Backend espera 'supplierName'
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

