import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-back-office-main',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <div class="container">
      <h1>Back Office</h1>
      <router-outlet></router-outlet>
    </div>
  `,
  styles: [`
    .container { padding: 1rem; }
  `]
})
export class BackOfficeMainComponent {}


