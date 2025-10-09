import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';

export interface UploadProfileImageResponse {
  userId: string;
  profileImageUrl: string;
}

@Injectable({
  providedIn: 'root',
})
export class ProfileImageService {
  constructor(private http: HttpClient, private apiConfig: ApiConfigService) {}

  /**
   * Upload profile image for a user
   */
  uploadProfileImage(userId: string, file: File): Observable<UploadProfileImageResponse> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    const url = `${this.apiConfig.getApiUrl()}/accounts/${userId}/upload-profile-image`;

    return this.http
      .post<{ value: UploadProfileImageResponse }>(url, formData)
      .pipe(map(response => response.value));
  }

  /**
   * Validate if file is a valid image
   */
  validateImage(file: File): { isValid: boolean; error?: string } {
    const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp', 'image/gif'];
    const maxSize = 5 * 1024 * 1024; // 5MB

    if (!allowedTypes.includes(file.type)) {
      return {
        isValid: false,
        error: 'Formato inválido. Apenas JPG, PNG, WEBP e GIF são permitidos.',
      };
    }

    if (file.size > maxSize) {
      return {
        isValid: false,
        error: 'Ficheiro muito grande. Tamanho máximo: 5MB.',
      };
    }

    return { isValid: true };
  }

  /**
   * Get default avatar URL
   */
  getDefaultAvatarUrl(): string {
    return 'assets/images/default-avatar.svg';
  }

  /**
   * Get profile image URL or default
   */
  getProfileImageUrl(profileImageUrl?: string | null): string {
    return profileImageUrl || this.getDefaultAvatarUrl();
  }
}
