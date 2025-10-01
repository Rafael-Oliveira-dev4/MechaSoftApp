export interface Employee {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phone: string;
  role: string;
  hourlyRate: number;
  specialties: string[];
  canPerformInspections: boolean;
  inspectionLicenseNumber?: string;
  isActive: boolean;
  hireDate: Date;
  createdAt: Date;
  updatedAt?: Date;
}

export interface CreateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  role: string;
  hourlyRate: number;
  specialties: string[];
  canPerformInspections: boolean;
  inspectionLicenseNumber?: string;
}

export interface UpdateEmployeeRequest {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  role: string;
  hourlyRate: number;
  specialties: string[];
  canPerformInspections: boolean;
  inspectionLicenseNumber?: string;
}

export interface EmployeesResponse {
  items: Employee[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

