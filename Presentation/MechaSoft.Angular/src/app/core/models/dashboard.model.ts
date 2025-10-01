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
}

export interface RecentOrder {
  id: string;
  orderNumber: string;
  customerName: string;
  vehiclePlate: string;
  status: string;
  createdAt: Date;
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

