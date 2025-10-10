import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Inspection {
  id: string;
  vehicleId: string;
  vehiclePlate: string;
  vehicleInfo: string;
  inspectionType: string;
  scheduledDate: Date;
  completedDate?: Date;
  expiryDate: Date;
  status: 'Agendada' | 'Concluída' | 'Vencida' | 'Próxima';
  result?: 'Aprovado' | 'Reprovado';
  observations?: string;
  inspectorName?: string;
}

@Component({
  selector: 'app-inspections',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inspections.component.html',
  styleUrls: ['./inspections.component.scss']
})
export class InspectionsComponent implements OnInit {
  inspections: Inspection[] = [];
  upcomingInspections: Inspection[] = [];
  completedInspections: Inspection[] = [];
  overdueInspections: Inspection[] = [];

  ngOnInit(): void {
    this.loadInspections();
  }

  loadInspections(): void {
    // Mock data - substituir com API real quando disponível
    this.inspections = [
      {
        id: '1',
        vehicleId: '1',
        vehiclePlate: '12-AB-34',
        vehicleInfo: 'BMW 320d',
        inspectionType: 'Inspeção Periódica Obrigatória',
        scheduledDate: new Date('2024-11-20T10:00:00'),
        expiryDate: new Date('2024-11-30'),
        status: 'Agendada',
        inspectorName: 'Centro de Inspeções MechaSoft'
      },
      {
        id: '2',
        vehicleId: '2',
        vehiclePlate: '56-CD-78',
        vehicleInfo: 'Volkswagen Golf 1.6 TDI',
        inspectionType: 'Inspeção Periódica Obrigatória',
        scheduledDate: new Date('2024-10-18T14:00:00'),
        expiryDate: new Date('2024-10-20'),
        status: 'Próxima',
        inspectorName: 'Centro de Inspeções MechaSoft'
      },
      {
        id: '3',
        vehicleId: '1',
        vehiclePlate: '12-AB-34',
        vehicleInfo: 'BMW 320d',
        inspectionType: 'Inspeção Periódica Obrigatória',
        scheduledDate: new Date('2023-11-15T09:00:00'),
        completedDate: new Date('2023-11-15T10:30:00'),
        expiryDate: new Date('2024-11-15'),
        status: 'Concluída',
        result: 'Aprovado',
        observations: 'Veículo em bom estado geral',
        inspectorName: 'Centro de Inspeções MechaSoft'
      },
      {
        id: '4',
        vehicleId: '2',
        vehiclePlate: '56-CD-78',
        vehicleInfo: 'Volkswagen Golf 1.6 TDI',
        inspectionType: 'Inspeção Periódica Obrigatória',
        scheduledDate: new Date('2022-10-10T11:00:00'),
        completedDate: new Date('2022-10-10T12:15:00'),
        expiryDate: new Date('2023-10-10'),
        status: 'Vencida',
        result: 'Aprovado',
        observations: 'Necessário agendar nova inspeção',
        inspectorName: 'Centro de Inspeções MechaSoft'
      }
    ];

    // Categorizar inspeções
    this.upcomingInspections = this.inspections.filter(i => i.status === 'Agendada' || i.status === 'Próxima');
    this.completedInspections = this.inspections.filter(i => i.status === 'Concluída');
    this.overdueInspections = this.inspections.filter(i => i.status === 'Vencida');
  }

  getDaysUntilExpiry(expiryDate: Date): number {
    const today = new Date();
    const expiry = new Date(expiryDate);
    const diffTime = expiry.getTime() - today.getTime();
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  getDaysSinceExpiry(expiryDate: Date): number {
    return Math.abs(this.getDaysUntilExpiry(expiryDate));
  }

  getStatusBadgeClass(status: string): string {
    switch (status) {
      case 'Agendada':
        return 'badge-blue';
      case 'Próxima':
        return 'badge-amber';
      case 'Concluída':
        return 'badge-green';
      case 'Vencida':
        return 'badge-red';
      default:
        return 'badge-gray';
    }
  }

  getResultBadgeClass(result?: string): string {
    return result === 'Aprovado' ? 'badge-green' : 'badge-red';
  }

  formatDate(date: Date | undefined): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('pt-PT', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}


