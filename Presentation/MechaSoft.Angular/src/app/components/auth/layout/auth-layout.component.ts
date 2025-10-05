import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-auth-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="auth-layout">
      <!-- Left Panel - Branding -->
      <div class="auth-branding">
        <div class="branding-content">
          <div class="logo-section">
            <svg class="logo" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"
              />
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
              />
            </svg>
            <h1 class="brand-title">MechaSoft</h1>
          </div>

          <div class="brand-description">
            <h2>Sistema de Gestão de Oficina</h2>
            <p>Gerencie sua oficina de forma profissional e eficiente</p>
          </div>

          <div class="features">
            <div class="feature-item">
              <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <span>Gestão de Clientes</span>
            </div>
            <div class="feature-item">
              <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <span>Ordens de Serviço</span>
            </div>
            <div class="feature-item">
              <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <span>Controle de Estoque</span>
            </div>
            <div class="feature-item">
              <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <span>Relatórios Completos</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Right Panel - Form -->
      <div class="auth-form-panel">
        <div class="form-container">
          <router-outlet></router-outlet>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .auth-layout {
        display: flex;
        min-height: 100vh;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      }

      .auth-branding {
        flex: 1;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 3rem;
        color: white;
      }

      .branding-content {
        max-width: 500px;
      }

      .logo-section {
        display: flex;
        align-items: center;
        gap: 1rem;
        margin-bottom: 3rem;
      }

      .logo {
        width: 60px;
        height: 60px;
        filter: drop-shadow(0 4px 6px rgba(0, 0, 0, 0.1));
      }

      .brand-title {
        font-size: 3rem;
        font-weight: 700;
        margin: 0;
      }

      .brand-description h2 {
        font-size: 1.875rem;
        font-weight: 600;
        margin: 0 0 1rem 0;
      }

      .brand-description p {
        font-size: 1.125rem;
        opacity: 0.9;
        line-height: 1.6;
      }

      .features {
        margin-top: 3rem;
        display: flex;
        flex-direction: column;
        gap: 1rem;
      }

      .feature-item {
        display: flex;
        align-items: center;
        gap: 0.75rem;
        font-size: 1rem;
      }

      .feature-item svg {
        width: 24px;
        height: 24px;
      }

      .auth-form-panel {
        flex: 1;
        display: flex;
        align-items: center;
        justify-content: center;
        background: white;
        padding: 3rem;
      }

      .form-container {
        width: 100%;
        max-width: 450px;
      }

      @media (max-width: 1024px) {
        .auth-branding {
          display: none;
        }

        .auth-form-panel {
          flex: 1;
        }
      }

      @media (max-width: 640px) {
        .auth-form-panel {
          padding: 1.5rem;
        }
      }
    `,
  ],
})
export class AuthLayoutComponent {}
