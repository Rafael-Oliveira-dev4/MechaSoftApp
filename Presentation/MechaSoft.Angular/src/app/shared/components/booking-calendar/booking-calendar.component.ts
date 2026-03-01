import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, computed, signal } from '@angular/core';

export interface CalendarDay {
  date: Date;
  isoDate: string;
  dayOfMonth: number;
  isCurrentMonth: boolean;
  isWeekend: boolean;
  isPast: boolean;
  isDisabled: boolean;
  isSelected: boolean;
  isToday: boolean;
}

@Component({
  selector: 'app-booking-calendar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './booking-calendar.component.html',
  styleUrls: ['./booking-calendar.component.scss'],
})
export class BookingCalendarComponent {
  @Input() selectedDate: string | null = null;
  @Input() disableWeekends = true;
  @Input() minDate: Date | string | null = null;
  @Input() maxDate: Date | string | null = null;

  @Output() dateSelected = new EventEmitter<string>();

  private displayDate = signal(new Date());
  private today = new Date();
  private todayStr = this.toIsoDate(this.today);
  /** Seleção local para atualizar a UI imediatamente ao clicar, sem depender do round-trip do parent */
  private localSelected = signal<string | null>(null);

  readonly weekDays = ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'];
  readonly monthNames = [
    'janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho',
    'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro'
  ];

  displayLabel = computed(() => {
    const d = this.displayDate();
    return `${this.monthNames[d.getMonth()]} de ${d.getFullYear()}`;
  });

  /** Data efetiva selecionada: input do parent ou seleção local (clique recente) */
  private effectiveSelected = computed(() => this.selectedDate ?? this.localSelected());

  calendarGrid = computed(() => this.buildGrid());

  private toIsoDate(d: Date): string {
    const y = d.getFullYear();
    const m = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${y}-${m}-${day}`;
  }

  private getMinDate(): Date {
    if (this.minDate != null) {
      const d = typeof this.minDate === 'string' ? new Date(this.minDate) : new Date(this.minDate.getTime());
      d.setHours(0, 0, 0, 0);
      return d;
    }
    const t = new Date(this.today.getTime());
    t.setHours(0, 0, 0, 0);
    return t;
  }

  private getMaxDate(): Date | null {
    if (this.maxDate == null) return null;
    const d = typeof this.maxDate === 'string' ? new Date(this.maxDate) : new Date(this.maxDate);
    d.setHours(23, 59, 59, 999);
    return d;
  }

  private buildGrid(): CalendarDay[][] {
    const display = this.displayDate();
    const year = display.getFullYear();
    const month = display.getMonth();
    const minD = this.getMinDate();
    const maxD = this.getMaxDate();

    const first = new Date(year, month, 1);
    const last = new Date(year, month + 1, 0);
    let startOffset = first.getDay();
    const daysInMonth = last.getDate();

    const rows: CalendarDay[][] = [];
    let row: CalendarDay[] = [];
    let dayIndex = 1 - startOffset;

    for (let r = 0; r < 6; r++) {
      row = [];
      for (let c = 0; c < 7; c++) {
        const d = new Date(year, month, dayIndex);
        const iso = this.toIsoDate(d);
        const dayOfMonth = d.getDate();
        const isCurrentMonth = d.getMonth() === month;
        const isWeekend = d.getDay() === 0 || d.getDay() === 6;
        const isPast = d < minD;
        const afterMax = maxD != null && d > maxD;
        const disabled = (!isCurrentMonth) || (this.disableWeekends && isWeekend) || isPast || afterMax;
        const effective = this.effectiveSelected();
        const isSelected = effective === iso;
        const isToday = iso === this.todayStr;

        row.push({
          date: d,
          isoDate: iso,
          dayOfMonth,
          isCurrentMonth,
          isWeekend,
          isPast,
          isDisabled: disabled,
          isSelected,
          isToday,
        });
        dayIndex++;
      }
      rows.push(row);
    }
    return rows;
  }

  prevMonth(): void {
    const d = this.displayDate();
    this.displayDate.set(new Date(d.getFullYear(), d.getMonth() - 1));
  }

  nextMonth(): void {
    const d = this.displayDate();
    this.displayDate.set(new Date(d.getFullYear(), d.getMonth() + 1));
  }

  goToday(): void {
    this.displayDate.set(new Date());
  }

  clearSelection(): void {
    this.localSelected.set(null);
    this.dateSelected.emit('');
  }

  onDayClick(day: CalendarDay, event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    if (day.isDisabled) return;
    this.localSelected.set(day.isoDate);
    this.dateSelected.emit(day.isoDate);
  }
}
