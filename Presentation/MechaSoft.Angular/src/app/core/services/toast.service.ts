import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  constructor(private toastr: ToastrService) {}

  success(message: string, title: string = 'Sucesso!'): void {
    this.toastr.success(message, title);
  }

  error(message: string, title: string = 'Erro!'): void {
    this.toastr.error(message, title);
  }

  warning(message: string, title: string = 'Atenção!'): void {
    this.toastr.warning(message, title);
  }

  info(message: string, title: string = 'Informação'): void {
    this.toastr.info(message, title);
  }

  // Atalhos específicos do negócio
  successCreate(entity: string): void {
    this.success(`${entity} criado(a) com sucesso!`);
  }

  successUpdate(entity: string): void {
    this.success(`${entity} atualizado(a) com sucesso!`);
  }

  successDelete(entity: string): void {
    this.success(`${entity} eliminado(a) com sucesso!`);
  }

  errorCreate(entity: string): void {
    this.error(`Erro ao criar ${entity}. Por favor, tente novamente.`);
  }

  errorUpdate(entity: string): void {
    this.error(`Erro ao atualizar ${entity}. Por favor, tente novamente.`);
  }

  errorDelete(entity: string): void {
    this.error(`Erro ao eliminar ${entity}. Por favor, tente novamente.`);
  }

  errorLoad(entity: string): void {
    this.error(`Erro ao carregar ${entity}. Por favor, recarregue a página.`);
  }
}

