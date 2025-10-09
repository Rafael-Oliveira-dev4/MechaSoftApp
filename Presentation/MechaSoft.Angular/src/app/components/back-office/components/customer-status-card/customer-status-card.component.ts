import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-customer-status-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './customer-status-card.component.html',
  styleUrls: ['./customer-status-card.component.scss'],
})
export class CustomerStatusCardComponent {
  @Input() hasCustomerProfile: boolean = false;
  @Input() customerName?: string;
  @Output() onClick = new EventEmitter<void>();

  handleClick(): void {
    if (!this.hasCustomerProfile) {
      this.onClick.emit();
    }
  }
}

