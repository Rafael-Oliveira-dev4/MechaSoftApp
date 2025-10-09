import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserModel } from '../core/models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly baseUrl = '/api/users';

  constructor(private readonly httpClient: HttpClient) {}

  // Stubs para integração futura
  getAll(): Observable<UserModel[]> {
    return this.httpClient.get<UserModel[]>(this.baseUrl);
  }
}


