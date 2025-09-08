import { UserRole } from './enums';

export interface UserModel {
  id: string;
  username: string;
  email: string;
  role: UserRole;
  isActive: boolean;
  emailConfirmed: boolean;
  lastLoginAt?: string;
  customerId?: string;
  employeeId?: string;
}


