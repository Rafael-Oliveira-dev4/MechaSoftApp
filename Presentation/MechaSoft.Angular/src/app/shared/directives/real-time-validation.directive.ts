import { Directive, ElementRef, HostListener, Input, OnInit, Renderer2 } from '@angular/core';
import { NgControl } from '@angular/forms';

/**
 * Diretiva para adicionar validação em tempo real a campos de formulário
 * Adiciona classes CSS baseadas no estado de validação do campo
 */
@Directive({
  selector: '[appRealTimeValidation]',
  standalone: true
})
export class RealTimeValidationDirective implements OnInit {
  @Input() showValidState: boolean = true; // Mostrar estado válido (verde)
  @Input() validateOnBlur: boolean = false; // Validar apenas no blur (padrão: validar enquanto digita)

  constructor(
    private el: ElementRef,
    private control: NgControl,
    private renderer: Renderer2
  ) {}

  ngOnInit() {
    if (this.control && this.control.control) {
      // Atualizar classes quando o valor mudar
      this.control.control.valueChanges?.subscribe(() => {
        if (!this.validateOnBlur) {
          this.updateValidationClasses();
        }
      });

      // Atualizar classes quando o status mudar
      this.control.control.statusChanges?.subscribe(() => {
        this.updateValidationClasses();
      });
    }
  }

  @HostListener('blur')
  onBlur() {
    this.updateValidationClasses();
  }

  @HostListener('focus')
  onFocus() {
    if (!this.validateOnBlur) {
      this.updateValidationClasses();
    }
  }

  private updateValidationClasses() {
    if (!this.control || !this.control.control) return;

    const control = this.control.control;
    const element = this.el.nativeElement;

    // Remove classes anteriores
    this.renderer.removeClass(element, 'field-valid');
    this.renderer.removeClass(element, 'field-invalid');

    // Adiciona classes baseadas no estado
    if (control.invalid && (control.dirty || control.touched)) {
      this.renderer.addClass(element, 'field-invalid');
    } else if (this.showValidState && control.valid && (control.dirty || control.touched)) {
      this.renderer.addClass(element, 'field-valid');
    }
  }
}

