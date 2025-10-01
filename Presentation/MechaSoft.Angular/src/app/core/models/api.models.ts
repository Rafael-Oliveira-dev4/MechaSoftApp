// Base interfaces
export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface ApiError {
  message: string;
  errors?: { [key: string]: string[] };
}

// Customer models
export interface Customer {
  id: string;
  name: string;
  email: string;
  phone: string;
  nif?: string;
  street: string;
  number: string;
  parish: string;
  city: string;
  postalCode: string;
  country: string;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CreateCustomerRequest {
  name: string;
  email: string;
  phone: string;
  nif?: string;
  street: string;
  number: string;
  parish: string;
  city: string;
  postalCode: string;
  country: string;
}

// Vehicle models
export interface Vehicle {
  id: string;
  customerId: string;
  customerName: string;
  brand: string;
  model: string;
  licensePlate: string;
  color: string;
  year: number;
  vin?: string;
  engineType?: string;
  fuelType: string;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CreateVehicleRequest {
  customerId: string;
  brand: string;
  model: string;
  licensePlate: string;
  color: string;
  year: number;
  vin?: string;
  engineType?: string;
  fuelType: string;
}

// Service Order models
export interface ServiceOrder {
  id: string;
  orderNumber: string;
  customerId: string;
  vehicleId: string;
  description: string;
  status: string;
  priority: string;
  estimatedCost: number;
  finalCost?: number;
  estimatedDelivery?: Date;
  actualDelivery?: Date;
  mechanicId?: string;
  actualHours?: number;
  requiresInspection: boolean;
  internalNotes?: string;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CreateServiceOrderRequest {
  customerId: string;
  vehicleId: string;
  description: string;
  priority: string;
  estimatedCost: number;
  estimatedDelivery?: Date;
  requiresInspection: boolean;
  internalNotes?: string;
}

// User/Auth models
export interface User {
  id: string;
  username: string;
  email: string;
  role: string;
  isActive: boolean;
  emailConfirmed: boolean;
  lastLoginAt?: Date;
  createdAt: Date;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  user: User;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
  role?: string;
}
