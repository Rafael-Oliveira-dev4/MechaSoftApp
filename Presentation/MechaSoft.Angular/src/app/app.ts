import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <app-navbar></app-navbar>
    <router-outlet></router-outlet>
  `,
  standalone: false,
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('MechaSoft.Angular');
}
