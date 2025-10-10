export interface DashboardStats {
  // Service Orders
  totalOrders: number;
  pendingOrders: number;
  inProgressOrders: number;
  completedOrders: number;
  
  // Revenue
  todayRevenue: number;
  monthRevenue: number;
  yearRevenue: number;
  
  // Customers & Vehicles
  totalCustomers: number;
  totalVehicles: number;
  
  // Parts
  lowStockPartsCount: number;
  totalPartsValue: number;
  
  // Inspections
  pendingInspections: number;
  
  // Recent activity (últimas 5)
  recentOrders: RecentOrder[];
  
  // Cliente specific
  nextAppointment?: NextAppointment;
  vehicles?: DashboardVehicle[];
  monthlyExpenses?: MonthlyExpense[];
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

