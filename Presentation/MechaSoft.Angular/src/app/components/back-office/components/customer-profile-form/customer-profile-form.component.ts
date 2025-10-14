import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CustomerType } from '../../../../core/models/enums';
import { AddressModel, NameModel } from '../../../../core/models/common';

export interface CustomerProfileFormData {
  firstName: string;
  lastName: string;
  phone: string;
  address: AddressModel;
  type: CustomerType;
  nif?: string;
  citizenCard?: string;
}

@Component({
  selector: 'app-customer-profile-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-profile-form.component.html',
  styleUrls: ['./customer-profile-form.component.scss'],
})
export class CustomerProfileFormComponent implements OnInit {
  @Input() initialData?: CustomerProfileFormData;
  @Input() isLoading: boolean = false;
  @Output() formSubmit = new EventEmitter<CustomerProfileFormData>();
  @Output() formCancel = new EventEmitter<void>();

  // Form data
  formData: CustomerProfileFormData = {
    firstName: '',
    lastName: '',
    phone: '',
    type: CustomerType.Individual,
    address: {
      street: '',
      number: '',
      parish: '',
      municipality: '',
      district: '',
      postalCode: '',
      complement: '',
    },
  };

  // Customer types for dropdown
  customerTypes = [
    { value: CustomerType.Individual, label: 'Particular' },
    { value: CustomerType.Company, label: 'Empresa' },
  ];

  // Validation errors
  errors: { [key: string]: string } = {};

  ngOnInit(): void {
    if (this.initialData) {
      this.formData = { ...this.initialData };
    }
  }

  onTypeChange(): void {
    // Clear name fields when switching type
    if (this.formData.type === 'Company') {
      // For companies, lastName should be empty
      this.formData.lastName = '';
    }
    // Clear errors
    delete this.errors['firstName'];
    delete this.errors['lastName'];
  }

  onSubmit(): void {
    if (this.validateForm()) {
      this.formSubmit.emit(this.formData);
    }
  }

  onCancel(): void {
    this.formCancel.emit();
  }

  private validateForm(): boolean {
    this.errors = {};
    let isValid = true;

    // Validate name based on customer type
    if (this.formData.type === 'Individual') {
      // For individuals: validate first and last name
      if (!this.formData.firstName.trim()) {
        this.errors['firstName'] = 'Primeiro nome é obrigatório';
        isValid = false;
      }
      if (!this.formData.lastName.trim()) {
        this.errors['lastName'] = 'Apelido é obrigatório';
        isValid = false;
      }
    } else {
      // For companies: validate company name (stored in firstName)
      if (!this.formData.firstName.trim()) {
        this.errors['firstName'] = 'Nome da empresa é obrigatório';
        isValid = false;
      }
      // Set lastName to empty string for companies
      this.formData.lastName = '';
    }

    // Validate phone
    if (!this.formData.phone.trim()) {
      this.errors['phone'] = 'Telefone é obrigatório';
      isValid = false;
    } else if (!this.validatePhone(this.formData.phone)) {
      this.errors['phone'] = 'Telefone inválido (mínimo 9 dígitos)';
      isValid = false;
    }

    // Validate address
    if (!this.formData.address.street.trim()) {
      this.errors['street'] = 'Rua é obrigatória';
      isValid = false;
    }
    if (!this.formData.address.number.trim()) {
      this.errors['number'] = 'Número é obrigatório';
      isValid = false;
    }
    if (!this.formData.address.parish.trim()) {
      this.errors['parish'] = 'Freguesia é obrigatória';
      isValid = false;
    }
    if (!this.formData.address.municipality.trim()) {
      this.errors['municipality'] = 'Concelho é obrigatório';
      isValid = false;
    }
    if (!this.formData.address.district.trim()) {
      this.errors['district'] = 'Distrito é obrigatório';
      isValid = false;
    }
    if (!this.formData.address.postalCode.trim()) {
      this.errors['postalCode'] = 'Código postal é obrigatório';
      isValid = false;
    } else if (!this.validatePostalCode(this.formData.address.postalCode)) {
      this.errors['postalCode'] = 'Código postal inválido (formato: XXXX-XXX)';
      isValid = false;
    }

    // Validate NIF if provided
    if (this.formData.nif && !this.validateNif(this.formData.nif)) {
      this.errors['nif'] = 'NIF inválido (deve ter 9 dígitos)';
      isValid = false;
    }

    return isValid;
  }

  private validatePhone(phone: string): boolean {
    const cleanPhone = phone.replace(/\s|-|\(|\)/g, '');
    return cleanPhone.length >= 9;
  }

  private validatePostalCode(postalCode: string): boolean {
    const cleanCode = postalCode.replace(/-|\s/g, '');
    return cleanCode.length === 7 && /^\d+$/.test(cleanCode);
  }

  private validateNif(nif: string): boolean {
    const cleanNif = nif.replace(/\s/g, '');
    return cleanNif.length === 9 && /^\d+$/.test(cleanNif);
  }

  // Format postal code as user types
  formatPostalCode(): void {
    let value = this.formData.address.postalCode.replace(/\D/g, '');
    if (value.length > 7) value = value.substring(0, 7);
    if (value.length > 4) {
      this.formData.address.postalCode = `${value.substring(0, 4)}-${value.substring(4)}`;
    } else {
      this.formData.address.postalCode = value;
    }
  }

  // Format NIF as user types
  formatNif(): void {
    if (this.formData.nif) {
      this.formData.nif = this.formData.nif.replace(/\D/g, '').substring(0, 9);
    }
  }

  hasError(field: string): boolean {
    return !!this.errors[field];
  }

  getError(field: string): string {
    return this.errors[field] || '';
  }
}

