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

// Response DTO para criação de Employee (inclui credenciais auto-geradas)
export interface CreateEmployeeResponse extends Employee {
  generatedUsername?: string;  // Username auto-gerado
  generatedPassword?: string;  // Senha temporária (retornada apenas uma vez)
}

