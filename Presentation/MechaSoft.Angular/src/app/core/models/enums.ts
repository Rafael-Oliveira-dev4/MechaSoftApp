export enum UserRole {
  Customer = 'Customer',
  Employee = 'Employee',
  Admin = 'Admin',
  Owner = 'Owner'
}

export enum CustomerType {
  Individual = 'Individual',
  Company = 'Company'
}

export enum FuelType {
  Gasoline = 'Gasoline',
  Diesel = 'Diesel',
  Electric = 'Electric',
  Hybrid = 'Hybrid',
  LPG = 'LPG',
  CNG = 'CNG'
}

export enum ServiceItemStatus {
  Pending = 'Pending',
  InProgress = 'InProgress',
  Paused = 'Paused',
  Completed = 'Completed',
  Cancelled = 'Cancelled'
}

export enum ServiceOrderStatus {
  Pending = 'Pending',
  InProgress = 'InProgress',
  WaitingParts = 'WaitingParts',
  WaitingApproval = 'WaitingApproval',
  WaitingInspection = 'WaitingInspection',
  Completed = 'Completed',
  Delivered = 'Delivered',
  Cancelled = 'Cancelled'
}

export enum Priority {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
  Urgent = 'Urgent'
}


