import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-profile-completion-alert',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './profile-completion-alert.component.html',
  styleUrls: ['./profile-completion-alert.component.scss'],
})
export class ProfileCompletionAlertComponent {
  @Input() isDismissible: boolean = false;
  
  isVisible: boolean = true;

  dismiss(): void {
    if (this.isDismissible) {
      this.isVisible = false;
    }
  }
}

