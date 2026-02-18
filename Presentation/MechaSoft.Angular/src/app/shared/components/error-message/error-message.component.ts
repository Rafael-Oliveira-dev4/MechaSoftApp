import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  Output,
  SimpleChanges,
} from '@angular/core';
import { ErrorDetail } from '../../../core/models/result.model';

@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './error-message.component.html',
  styleUrls: ['./error-message.component.scss'],
})
export class ErrorMessageComponent implements OnChanges, OnDestroy {
  @Input() error: ErrorDetail | null = null;
  @Input() autoDismiss: boolean = true;
  @Input() dismissTime: number = 5000; // 5 segundos
  @Output() onClose = new EventEmitter<void>();

  private dismissTimeout?: ReturnType<typeof setTimeout>;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['error'] && this.error && this.autoDismiss) {
      this.startAutoDismiss();
    }
  }

  ngOnDestroy(): void {
    this.clearAutoDismiss();
  }

  private startAutoDismiss(): void {
    this.clearAutoDismiss();
    this.dismissTimeout = setTimeout(() => {
      this.close();
    }, this.dismissTime);
  }

  private clearAutoDismiss(): void {
    if (this.dismissTimeout) {
      clearTimeout(this.dismissTimeout);
      this.dismissTimeout = undefined;
    }
  }

  close(): void {
    this.clearAutoDismiss();
    this.onClose.emit();
  }
}
