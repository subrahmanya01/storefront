import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ProductVariant } from '../../../interfaces/product-meta';
import { ProductService } from '../../../services/product.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-variant-image-manager',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './variant-image-manager.component.html',
  styleUrl: './variant-image-manager.component.css'
})
export class VariantImageManagerComponent implements OnInit {
  @Input() productId!: string | undefined; 
  @Input() variant: ProductVariant | null = null; 
  @Output() imagesUpdated = new EventEmitter<ProductVariant>(); 
  @Output() cancel = new EventEmitter<void>(); 

  selectedFiles: File[] = []; 
  isUploading: boolean = false; 
  isDraggingOver: boolean = false; 

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.selectedFiles = [...this.selectedFiles, ...Array.from(input.files)];
      console.log('Files selected:', this.selectedFiles.map(f => f.name));
       input.value = '';
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault(); 
    event.stopPropagation();
    this.isDraggingOver = true; 
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDraggingOver = false; 
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDraggingOver = false; 

    const files = event.dataTransfer?.files; 

    if (files && files.length > 0) {
       const imageFiles = Array.from(files).filter(file => file.type.startsWith('image/'));
       this.selectedFiles = [...this.selectedFiles, ...imageFiles];
       console.log('Files dropped:', imageFiles.map(f => f.name));
    }
  }


  uploadImages(): void {
    if (this.selectedFiles.length === 0 || !this.variant?.id || !this.productId || this.isUploading) {
      console.warn('No files selected for upload or missing context or upload already in progress.');
      return;
    }

    this.isUploading = true;

    const formData = new FormData();
    for (const file of this.selectedFiles) {
      formData.append('files', file, file.name);
    }
    this.productService.uploadImagesForVariant(this.productId, this.variant.id, formData).subscribe(
      {
        next:(uploadedImageUrls: string[]) => { 
          if (this.variant) {
            if (!this.variant.images) {
               this.variant.images = [];
            }
            this.variant.images.push(...uploadedImageUrls); 
            this.selectedFiles = []; 
            console.log('Images uploaded successfully (mock)', uploadedImageUrls);
          }
           this.isUploading = false; 
      },
      error:(error:any) => {
         console.error('Error uploading images', error);
         alert('Failed to upload images.'); 
         this.isUploading = false; 
      }
     }
       
    );
  }

  removeImage(imageUrl: string): void {
    if (this.variant?.id && this.productId && imageUrl) {
      console.log(`Attempting to remove image URL: ${imageUrl}`);
      this.productService.removeImageFromVariant(this.productId, this.variant.id, [imageUrl]).subscribe(
        {
          next:() => {
            if (this.variant && this.variant.images) {
              this.variant.images = this.variant.images.filter(img => img !== imageUrl);
            }
          },
          error:(error:any) => {
            console.error('Error removing image', error);
          }
        }
      );
    } else {
       console.warn('Image URL or variant/product ID is missing.');
    }
  }

  onDone(): void {
     if (this.variant) {
       this.imagesUpdated.emit(this.variant);
     }
    this.cancel.emit();
  }

   getAttributeSummary(variant: ProductVariant): string {
      if (!variant || !variant.attributes) return 'No Attributes';
      const attributes = Object.entries(variant.attributes)
          .map(([key, value]) => `${key}: ${value}`)
          .join(', '); 
      return attributes || 'No Attributes';
   }
}
