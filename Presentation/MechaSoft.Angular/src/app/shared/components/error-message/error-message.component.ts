import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorDetail } from '../../../core/models/result.model';

@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './error-message.component.html',
  styleUrls: ['./error-message.component.scss']
})
export class ErrorMessageComponent {
  @Input() error: ErrorDetail | null = null;
  @Output() onClose = new EventEmitter<void>();
  
  close(): void {
    this.onClose.emit();
  }
}
