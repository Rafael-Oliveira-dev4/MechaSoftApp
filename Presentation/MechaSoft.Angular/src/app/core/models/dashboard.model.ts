export interface DashboardStats {
  totalCustomers: number;
  activeCustomers: number;
  totalVehicles: number;
  totalEmployees: number;
  activeMechanics: number;
  serviceOrders: ServiceOrderStats;
  parts: PartStats;
  monthRevenue: number;
  todayRevenue: number;
  
  // Legacy properties for backward compatibility (optional)
  totalOrders?: number;
  pendingOrders?: number;
  inProgressOrders?: number;
  completedOrders?: number;
  yearRevenue?: number;
  lowStockPartsCount?: number;
  totalPartsValue?: number;
  pendingInspections?: number;
  recentOrders?: RecentOrder[];
  nextAppointment?: NextAppointment;
  vehicles?: DashboardVehicle[];
  monthlyExpenses?: MonthlyExpense[];
}

export interface ServiceOrderStats {
  total: number;
  pending: number;
  inProgress: number;
  waitingParts: number;
  waitingInspection: number;
  completedToday: number;
  completedThisMonth: number;
}

export interface PartStats {
  totalParts: number;
  lowStockParts: number;
  totalInventoryValue: number;
  criticalLowStock: CriticalLowStockPart[];
}

export interface CriticalLowStockPart {
  partId: string;
  code: string;
  name: string;
  currentStock: number;
  minStock: number;
  deficit: number;
}

export interface RecentOrder {
  id: string;
  orderNumber: string;
  customerName?: string;
  vehiclePlate: string;
  service?: string;
  status: string;
  createdAt: Date;
}

export interface NextAppointment {
  date: Date;
  service: string;
  vehicle: string;
}

export interface DashboardVehicle {
  id: string;
  plate: string;
  brand: string;
  model: string;
  year: number;
  lastService?: string;
}

export interface MonthlyExpense {
  month: string;
  amount: number;
}

export interface LowStockReport {
  parts: LowStockPart[];
  totalDeficit: number;
  criticalCount: number; // deficit > 50%
}

interface LowStockPart {
  id: string;
  partNumber: string;
  name: string;
  quantityInStock: number;
  minimumStock: number;
  deficit: number;
  deficitPercentage: number;
}

