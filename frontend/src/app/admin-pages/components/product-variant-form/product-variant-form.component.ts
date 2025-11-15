import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { ProductVariant } from '../../../interfaces/product-meta';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../../services/product.service';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from '../../../building-blocks/button/button.component';
import { SnackbarService } from '../../../services/snackbar.service';

@Component({
  selector: 'app-product-variant-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonComponent
  ],
  templateUrl: './product-variant-form.component.html',
  styleUrl: './product-variant-form.component.css'
})
export class ProductVariantFormComponent implements OnInit, OnChanges {
  @Input() productId!: string | undefined; 
  @Input() variant: ProductVariant | null = null; 
  @Input() allowedAttributes: string[] = []; 
  @Output() variantSaved:any = new EventEmitter<ProductVariant>();
  @Output() cancel = new EventEmitter<void>();

  variantForm!: FormGroup;

  constructor(private fb: FormBuilder, private productService: ProductService, private readonly snackbarService: SnackbarService) { }

  ngOnInit(): void {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
     if (changes['variant'] || changes['allowedAttributes']) {
        this.initForm(); 
     }
  }

  initForm(): void {
    
    const attributeControls: { [key: string]: AbstractControl } = {};
    this.allowedAttributes.forEach(attr => {
      attributeControls[attr] = new FormControl(this.variant?.attributes?.[attr] || '', Validators.required);
    });

    this.variantForm = this.fb.group({
      id: [this.variant?.id || null],
      attributes: this.fb.group(attributeControls), 
      price: [this.variant?.price || null, [Validators.required, Validators.min(0)]],
      inventory: [this.variant?.inventory || null, [Validators.required, Validators.min(0)]],
      
      images: [this.variant?.images || []] 
    });
  }

  onSubmit(): void {
    if (this.variantForm.valid && this.productId) {
      const variantData: ProductVariant = this.variantForm.value;

      if (variantData.id) {
        
        this.productService.updateVariantForProduct(this.productId, variantData).subscribe(
          updatedVariant => {
            this.variantSaved.emit(updatedVariant);
            this.snackbarService.success("Variant updated successfully");
          },
          error => {
            this.snackbarService.error("Error updating variant");
          }
        );
      } else {
        
        this.productService.addVariantToProduct(this.productId, variantData).subscribe(
          newVariant => {
            this.variantSaved.emit(newVariant);
            this.snackbarService.success("Variant created successfully");
          },
          error => {
            this.snackbarService.error("Error creating variant");
          }
        );
      }
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
