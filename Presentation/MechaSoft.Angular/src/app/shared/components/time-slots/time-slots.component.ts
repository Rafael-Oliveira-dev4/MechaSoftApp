import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, computed } from '@angular/core';

export interface TimeSlotsConfig {
  startHour: number;
  endHour: number;
  intervalMinutes: number;
}

const DEFAULT_CONFIG: TimeSlotsConfig = {
  startHour: 9,
  endHour: 18,
  intervalMinutes: 60,
};

@Component({
  selector: 'app-time-slots',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './time-slots.component.html',
  styleUrls: ['./time-slots.component.scss'],
})
export class TimeSlotsComponent {
  @Input() selectedDate: string | null = null;
  @Input() selectedTime: string | null = null;
  @Input() config: TimeSlotsConfig = DEFAULT_CONFIG;

  /** Opcional: slots já ocupados (ex.: ['09:00','10:30']) para desativar */
  @Input() disabledSlots: string[] = [];

  @Output() slotSelected = new EventEmitter<string>();

  readonly slots = computed(() => this.buildSlots());

  private buildSlots(): string[] {
    const { startHour, endHour, intervalMinutes } = this.config;
    const list: string[] = [];
    for (let h = startHour; h < endHour; h++) {
      for (let m = 0; m < 60; m += intervalMinutes) {
        if (h === endHour - 1 && m + intervalMinutes > 60) break;
        const hour = String(h).padStart(2, '0');
        const min = String(m).padStart(2, '0');
        list.push(`${hour}:${min}`);
      }
    }
    return list;
  }

  isDisabled(slot: string): boolean {
    return this.disabledSlots.includes(slot);
  }

  isSelected(slot: string): boolean {
    return this.selectedTime === slot;
  }

  onSlotClick(slot: string): void {
    if (this.isDisabled(slot)) return;
    this.slotSelected.emit(slot);
  }

  formatSlot(slot: string): string {
    return slot;
  }
}
