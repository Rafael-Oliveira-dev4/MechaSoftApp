import { Component } from '@angular/core';

@Component({
  selector: 'app-error',
  standalone: true,
  template: `
    <div class="container">
      <h2>Página não encontrada</h2>
      <p>Verifique o endereço.</p>
    </div>
  `,
  styles: [`
    .container { padding: 1rem; }
  `]
})
export class ErrorComponent {}


