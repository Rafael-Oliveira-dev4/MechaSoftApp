import { Component, OnInit, AfterViewInit, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-landing',
  standalone: false,
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit, AfterViewInit {
  isMobileMenuOpen = false;
  isNavbarScrolled = false;
  
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}
  
  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.setupScrollListener();
    }
  }

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  closeMobileMenu() {
    this.isMobileMenuOpen = false;
  }

  private setupScrollListener() {
    if (typeof window === 'undefined') return;
    
    window.addEventListener('scroll', () => {
      this.isNavbarScrolled = window.scrollY > 50;
    }, { passive: true });
  }

  scrollToSection(sectionId: string) {
    if (typeof document === 'undefined') return;
    
    const element = document.querySelector(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
      this.closeMobileMenu();
    }
  }

  ngAfterViewInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.animateCounters();
      this.animateOnScroll();
    }
  }

  private animateOnScroll() {
    if (typeof document === 'undefined') return;
    
    const elements = document.querySelectorAll('[data-animate]');
    
    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.classList.add('animate-fadeInUp');
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.1 });

    elements.forEach(el => observer.observe(el));
  }

  private animateCounters() {
    if (typeof document === 'undefined') return;
    
    const counters = document.querySelectorAll('[data-count]');
    
    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          const counter = entry.target as HTMLElement;
          const target = parseFloat(counter.getAttribute('data-count') || '0');
          this.animateCounter(counter, target);
          observer.unobserve(counter);
        }
      });
    }, { threshold: 0.5 });

    counters.forEach(counter => observer.observe(counter));
  }

  private animateCounter(element: HTMLElement, target: number) {
    const duration = 2000; // 2 seconds
    const start = 0;
    const increment = target / (duration / 16); // 60fps
    let current = start;
    const format = element.getAttribute('data-format') || 'number';

    const timer = setInterval(() => {
      current += increment;
      if (current >= target) {
        current = target;
        clearInterval(timer);
      }
      
      const value = Math.floor(current);
      
      switch(format) {
        case 'number+':
          element.textContent = value + '+';
          break;
        case 'k+':
          element.textContent = (value / 1000).toFixed(0) + 'K+';
          break;
        case 'decimal':
          element.textContent = current.toFixed(1) + '★';
          break;
        case 'percent':
          element.textContent = value + '%';
          break;
        default:
          element.textContent = value.toString();
      }
    }, 16);
  }
}


