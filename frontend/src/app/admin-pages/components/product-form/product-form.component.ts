import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { Product } from '../../../interfaces/product-meta';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService } from '../../../services/product.service';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from '../../../building-blocks/button/button.component';
import { SnackbarService } from '../../../services/snackbar.service';

@Component({
  selector: 'app-product-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonComponent
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent implements OnInit, OnChanges{
  @Input() product: Product | null = null;
  @Output() productSaved:any = new EventEmitter<Product>();

  productForm!: FormGroup;

  constructor(private fb: FormBuilder, private productService: ProductService, private readonly snackbarService: SnackbarService) { }

  ngOnInit(): void {
    this.initForm();
    if (this.product) {
      this.patchForm();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['product'] && this.productForm) { 
      console.log('Product input changed in ngOnChanges. Form is initialized. Updating form.');
      if (this.product) {
        this.patchForm();
      } else {
        this.initForm();
      }
    } else if (changes['product'] && !this.productForm) {
        console.log('Product input changed in ngOnChanges BEFORE form initialization. Will be handled by ngOnInit.');
    }
  }

  initForm(): void {
    this.productForm = this.fb.group({
      id: [null],
      name: ['', Validators.required],
      type: ['', Validators.required],
      category: ['', Validators.required],
      description: ['', Validators.required],
      allowedAttributes: this.fb.array([]),
      baseAttributes: this.fb.group({
         'Brand': ['']
      }),

    });
  }

  patchForm(): void {
    if (this.product) {
      this.productForm?.patchValue({
        id: this.product.id,
        name: this.product.name,
        type: this.product.type,
        category: this.product.category,
        description: this.product.description,
        baseAttributes: this.product.baseAttributes
      });

      this.allowedAttributes.clear();
      if (this.product.allowedAttributes) {
        this.product.allowedAttributes.forEach(attr => {
          this.allowedAttributes.push(this.fb.control(attr));
        });
      }
    }
  }

  get allowedAttributes(): FormArray {
    return this.productForm.get('allowedAttributes') as FormArray;
  }

  addAllowedAttribute(): void {
    this.allowedAttributes.push(this.fb.control('', Validators.required));
  }

  removeAttribute(index: number): void {
     if (this.allowedAttributes.length >0) { 
        this.allowedAttributes.removeAt(index);
     } else {
        this.allowedAttributes.at(index).reset(); 
     }
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      const productData: Product = this.productForm.value;

      if (productData.id) {
        this.productService.updateProduct(productData).subscribe(
          {
            next:(updatedProduct:Product) => {
              this.productSaved.emit(updatedProduct);
              this.snackbarService.success("Product updated successfully");
            },
            error:(error:any) => {
              this.snackbarService.error("Error updating product");
            }
          }
        );
      } else {
        this.productService.createProduct(productData).subscribe(
          {
            next:(newProduct:Product) => {
              this.productSaved.emit(newProduct);
              this.snackbarService.success("Product created successfully");
              this.productForm.patchValue({ id: newProduct.id });
            },
            error: (error: any) => {
              this.snackbarService.error("Error creating product");
            }
          },
       
        );
      }
    }
  }
}
