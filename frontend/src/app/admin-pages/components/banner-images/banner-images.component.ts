import { CommonModule } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ProductInfoService } from '../../../services/product-info.service';

interface ExistingImage {
  id: string;
  url: string;
  name?: string;
}

@Component({
  selector: 'app-banner-images',
  imports: [
    CommonModule
  ],
  templateUrl: './banner-images.component.html',
  styleUrl: './banner-images.component.css'
})
export class BannerImagesComponent {
  @ViewChild('fileInput') fileInput!: ElementRef; 
  selectedFiles: File[] = []; 
  existingImages: ExistingImage[] = []; 

  isDragging: boolean = false; 
  isUploading: boolean = false; 
  isLoadingExisting: boolean = false; 
  isDeleting: { [id: string]: boolean } = {};

  uploadMessage: string = '';

  constructor(private readonly productInfoService: ProductInfoService) { }

  ngOnInit(): void {
    this.loadExistingImages();
  }

  ngOnDestroy(): void {
    this.selectedFiles.forEach(file => URL.revokeObjectURL(this.getFileUrl(file)));
  }

  loadExistingImages(): void {
    this.isLoadingExisting = true;
    this.productInfoService.getBannerImages().subscribe({
      next:(data:string[])=>{
        for(let i=0; i< data.length; i++)
        {
          this.existingImages.push({
            id: i+1,
            name: `image_${i+1}`,
            url: data[i]
          } as any)
        }
        this.isLoadingExisting = false;
      },
      error:(err: any)=>{
        console.error(err.message);
      }
    })
  }

  onFileSelected(event: any): void {
    const files: FileList | null = event.target.files;
    this.addFiles(files);
    if (this.fileInput) {
      this.fileInput.nativeElement.value = '';
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault(); 
    event.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    const files: FileList | null = event.dataTransfer?.files || null;
    this.addFiles(files);
  }

  private addFiles(fileList: FileList | null): void {
    if (fileList) {
      const filesArray: File[] = Array.from(fileList);
      const imageFiles = filesArray.filter(file => file.type.startsWith('image/'));
      this.selectedFiles.push(...imageFiles);
      this.uploadMessage = ''; 
    }
  }
  getFileUrl(file: File): string {
    return URL.createObjectURL(file);
  }

  removeSelectedFile(index: number): void {
    URL.revokeObjectURL(this.getFileUrl(this.selectedFiles[index]));
    this.selectedFiles.splice(index, 1);
    if (this.selectedFiles.length === 0) {
        this.uploadMessage = ''; 
    }
  }

  removeExistingImage(imageId: string): void {
     if (this.isDeleting[imageId]) {
        return; 
     }
     this.isDeleting[imageId] = true; 

    console.log('Simulating deleting existing image with ID:', imageId);

    const indexToRemove = this.existingImages.findIndex(img => img.id === imageId);
    
    if(indexToRemove != -1)
    {
      this.productInfoService.removeBannerImage(this.existingImages[indexToRemove].url).subscribe({
        next: (data:any) => {
          this.existingImages.splice(indexToRemove, 1);
        },
        error: (err) => {
          console.error(err.message);
        }
      });
    }
  }

  uploadFiles(): void {
    if (this.selectedFiles.length === 0 || this.isUploading) {
      console.warn('No files selected for upload or already uploading.');
      return;
    }

    this.isUploading = true;
    this.uploadMessage = 'Uploading...';

    const formData = new FormData();
    for (const file of this.selectedFiles) {
      formData.append('files', file, file.name);
    }
  
    this.productInfoService.uploadBannerImages(formData).subscribe({
      next: (data:string[])=>{
        const exLength = this.existingImages.length;
        for(let i=0; i< data.length; i++)
        {
         this.existingImages.push({
          id: i+exLength,
          name: `image_${i+exLength}`,
          url: data[i]
         } as any);

        }
        this.selectedFiles = [];
        this.uploadMessage = 'Upload successful!';
        this.isUploading = false;
      },
      error:(err: any)=>{
        console.error(err.message)
      }
    })
  }


  trackById(index: number, item: ExistingImage): string {
    return item.id; 
  }
}
