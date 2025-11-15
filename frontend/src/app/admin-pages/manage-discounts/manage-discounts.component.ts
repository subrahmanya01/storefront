import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Discount, DiscountRequest } from '../../interfaces/discounts';
import { DiscountsService } from '../../services/discounts.service';
import { catchError, of, tap } from 'rxjs';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-manage-discounts',
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule
  ],
  templateUrl: './manage-discounts.component.html',
  styleUrl: './manage-discounts.component.css'
})
export class ManageDiscountsComponent implements OnInit{

  discountForm: FormGroup;
  discounts: Discount[] = [];
  editingDiscount: Discount | null = null;
  loading: boolean = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder, 
    private snackbar: SnackbarService,
    private discountService: DiscountsService 
  ) {
    this.discountForm = this.fb.group({
      code: [null], 
      percentage: [null, [Validators.required, Validators.min(0), Validators.max(100)]], 
      minOrderAmount: [null],
      category: [null],
      productId: [null],
      validFrom: [null, Validators.required], 
      validTo: [null, Validators.required] 
    });
  }

  ngOnInit(): void {
    this.loadDiscounts();
  }

  loadDiscounts(): void {
    this.loading = true; 
    this.error = null; 
    this.discountService.getAllDiscounts()
      .pipe(
        tap(discounts => {
          this.discounts = discounts as any; 
          this.loading = false; 
        }),
        catchError(err => {
          this.error = 'Failed to load discounts.'; 
          this.loading = false; 
          console.error('Error loading discounts:', err); 
          return of([]); 
        })
      )
      .subscribe(); 
  }

  onSubmit(): void {
    if (this.discountForm.invalid) {
      this.discountForm.markAllAsTouched();
      return; 
    }

    const formData: DiscountRequest = this.discountForm.value;

    if (formData.validFrom) {
      formData.validFrom = new Date(formData.validFrom).toISOString();
    } else {
      formData.validFrom = null as any;
    }

    if (formData.validTo) {
      formData.validTo = new Date(formData.validTo).toISOString();
    } else {
      formData.validTo = null as any;
    }

    if (this.editingDiscount) {
      this.updateDiscount(this.editingDiscount.id, formData);
    } else {
      this.createDiscount(formData);
    }
  }

  
  createDiscount(discount: DiscountRequest): void {
    this.loading = true; 
    this.error = null; 
    this.discountService.createDiscountEntry(discount)
      .pipe(
        
        tap((newDiscount:any) => {
          this.discounts.push(newDiscount); 
          this.resetForm(); 
          this.loading = false; 
        }),
        
        catchError(err => {
          this.error = 'Failed to create discount.'; 
          this.loading = false; 
          console.error('Error creating discount:', err); 
          return of(undefined); 
        })
      )
      .subscribe(); 
  }

  
  updateDiscount(id: string, discount: DiscountRequest): void {
    this.loading = true; 
    this.error = null; 
    
    this.discountService.updateDiscountEntry(id, discount)
      .pipe(
        
        tap((updatedDiscount:any) => {
          
          const index = this.discounts.findIndex(d => d.id === updatedDiscount.id);
          if (index !== -1) {
            
            this.discounts[index] = updatedDiscount;
          }
          this.resetForm(); 
          this.loading = false; 
        }),
        
        catchError(err => {
          this.error = 'Failed to update discount.'; 
          this.loading = false; 
          console.error('Error updating discount:', err); 
          return of(undefined); 
        })
      )
      .subscribe(); 
  }

  
  deleteDiscount(id: string): void {
    
    if (confirm('Are you sure you want to delete this discount?')) {
      this.loading = true; 
      this.error = null; 
      
      this.discountService.deleteEntry(id)
        .pipe(
          
          tap(success => {
            if (success) {
              
              this.snackbar.success("Discount entry deleted successfully")
              this.discounts = this.discounts.filter(discount => discount.id !== id);
            } else {
              
              this.error = 'Failed to delete discount.';
            }
            this.loading = false; 
          }),
          
          catchError(err => {
            this.error = 'Failed to delete discount.'; 
            this.loading = false; 
            console.error('Error deleting discount:', err); 
            return of(false); 
          })
        )
        .subscribe(); 
    }
  }

  
  editDiscount(discount: Discount): void {
    this.editingDiscount = discount; 
    
    this.discountForm.patchValue({
      code: discount.code,
      percentage: discount.percentage,
      minOrderAmount: discount.minOrderAmount,
      category: discount.category,
      productId: discount.productId,
      
      validFrom: discount.validFrom ? new Date(discount.validFrom).toISOString().split('T')[0] : null,
      validTo: discount.validTo ? new Date(discount.validTo).toISOString().split('T')[0] : null
    });
  }

  
  resetForm(): void {
    this.discountForm.reset({
      code: null,
      percentage: null,
      minOrderAmount: null,
      category: null,
      productId: null,
      validFrom: null,
      validTo: null
    });
    this.editingDiscount = null; 
  }

  
  cancelEdit(): void {
    this.resetForm(); 
  }
}
