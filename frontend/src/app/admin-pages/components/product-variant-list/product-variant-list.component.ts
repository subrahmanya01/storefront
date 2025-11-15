import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { ProductVariant } from '../../../interfaces/product-meta';
import { ProductService } from '../../../services/product.service';
import { CommonModule } from '@angular/common';
import { ProductVariantFormComponent } from '../product-variant-form/product-variant-form.component';
import { VariantImageManagerComponent } from '../variant-image-manager/variant-image-manager.component';
import { ButtonComponent } from '../../../building-blocks/button/button.component';

@Component({
  selector: 'app-product-variant-list',
  imports: [
    CommonModule,
    ProductVariantFormComponent,
    VariantImageManagerComponent,
    ButtonComponent
  ],
  templateUrl: './product-variant-list.component.html',
  styleUrl: './product-variant-list.component.css'
})
export class ProductVariantListComponent implements OnChanges{
  @Input() productId!: string|undefined; 
  @Input() productVariants: ProductVariant[] = []; 
  @Input() allowedAttributes: string[] = []; 
  @Output() variantsUpdated:any = new EventEmitter<ProductVariant[]>(); 

  variants: ProductVariant[] = []; 

  showVariantForm: boolean = false;
  editingVariant: ProductVariant | null = null; 

  showImageManager: boolean = false;
  selectedVariantForImages: ProductVariant | null = null; 

  constructor(private productService: ProductService) { }

  ngOnChanges(changes: SimpleChanges): void {
    
    if (changes['productVariants'] && this.productVariants) {
      this.variants = [...this.productVariants]; 
    }
    
  }

  startAddingVariant(): void {
    this.editingVariant = null; 
    this.showVariantForm = true;
  }

  startEditingVariant(variant: ProductVariant): void {
    
    this.editingVariant = { ...variant, attributes: { ...variant.attributes }, images: [...variant.images] };
    this.showVariantForm = true;
  }

  onVariantSaved(savedVariant: ProductVariant): void {
    
    if (savedVariant.id) {
      
      const index = this.variants.findIndex(v => v.id === savedVariant.id);
      if (index !== -1) {
        this.variants[index] = savedVariant; 
      }
    } else {
      
      this.variants.push(savedVariant);
    }
    this.showVariantForm = false; 
    this.editingVariant = null; 
    this.emitVariantsUpdate(); 
  }

  cancelVariantForm(): void {
    this.showVariantForm = false; 
    this.editingVariant = null; 
  }

  deleteVariant(variantId: string): void {
    if (confirm('Are you sure you want to delete this variant?')) {
      this.productService.deleteVariantFromProduct(this.productId??"", variantId).subscribe(
        () => {
          
          this.variants = this.variants.filter(v => v.id !== variantId);
          console.log('Variant deleted successfully', variantId);
          this.emitVariantsUpdate(); 
        },
        error => {
          console.error('Error deleting variant', error);
          alert('Failed to delete variant.'); 
        }
      );
    }
  }

  startManagingImages(variant: ProductVariant): void {
     this.selectedVariantForImages = variant; 
     this.showImageManager = true; 
  }

  onVariantImagesUpdated(updatedVariant: ProductVariant): void {
     
     
     const index = this.variants.findIndex(v => v.id === updatedVariant.id);
      if (index !== -1) {
         this.variants[index].images = updatedVariant.images; 
      }
     this.cancelImageManager(); 
     this.emitVariantsUpdate(); 
  }

  cancelImageManager(): void {
     this.showImageManager = false; 
     this.selectedVariantForImages = null; 
  }

  
  private emitVariantsUpdate(): void {
     
     this.variantsUpdated.emit([...this.variants]);
  }

   
   getAttributeSummary(variant: ProductVariant): string {
      if (!variant || !variant.attributes) return 'No Attributes';
      const attributes = Object.entries(variant.attributes)
          .map(([key, value]) => `${key}: ${value}`)
          .join(', ');
      return attributes || 'No Attributes';
   }
}
