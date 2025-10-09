import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProfileImageService } from '../../../core/services/profile-image.service';

@Component({
  selector: 'app-profile-image-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile-image-upload.component.html',
  styleUrls: ['./profile-image-upload.component.scss'],
})
export class ProfileImageUploadComponent {
  @Input() userId!: string;
  @Input() currentImageUrl?: string | null;
  @Output() uploadSuccess = new EventEmitter<string>();
  @Output() uploadError = new EventEmitter<string>();

  imagePreview: string | null = null;
  selectedFile: File | null = null;
  isUploading = false;
  isDragging = false;
  errorMessage: string | null = null;

  constructor(private profileImageService: ProfileImageService) {}

  /**
   * Handle file selection from input
   */
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.handleFile(input.files[0]);
    }
  }

  /**
   * Handle drag over event
   */
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  /**
   * Handle drag leave event
   */
  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  /**
   * Handle file drop event
   */
  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      this.handleFile(event.dataTransfer.files[0]);
    }
  }

  /**
   * Handle file validation and preview
   */
  private handleFile(file: File): void {
    this.errorMessage = null;

    // Validate file
    const validation = this.profileImageService.validateImage(file);
    if (!validation.isValid) {
      this.errorMessage = validation.error || 'Ficheiro inválido';
      return;
    }

    // Store selected file
    this.selectedFile = file;

    // Create preview
    const reader = new FileReader();
    reader.onload = (e: ProgressEvent<FileReader>) => {
      this.imagePreview = e.target?.result as string;
    };
    reader.readAsDataURL(file);
  }

  /**
   * Upload the selected image
   */
  uploadImage(): void {
    if (!this.selectedFile || !this.userId) {
      return;
    }

    this.isUploading = true;
    this.errorMessage = null;

    this.profileImageService.uploadProfileImage(this.userId, this.selectedFile).subscribe({
      next: response => {
        this.isUploading = false;
        this.currentImageUrl = response.profileImageUrl;
        this.imagePreview = null;
        this.selectedFile = null;
        this.uploadSuccess.emit(response.profileImageUrl);
      },
      error: err => {
        this.isUploading = false;
        this.errorMessage = err?.error?.message || 'Erro ao fazer upload da imagem';
        this.uploadError.emit(this.errorMessage || 'Erro desconhecido');
      },
    });
  }

  /**
   * Cancel image selection
   */
  cancelSelection(): void {
    this.imagePreview = null;
    this.selectedFile = null;
    this.errorMessage = null;
  }

  /**
   * Get current display image URL
   */
  get displayImageUrl(): string {
    return this.profileImageService.getProfileImageUrl(this.currentImageUrl);
  }

  /**
   * Check if has preview
   */
  get hasPreview(): boolean {
    return !!this.imagePreview;
  }
}
