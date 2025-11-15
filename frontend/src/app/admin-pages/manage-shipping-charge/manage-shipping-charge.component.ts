import { Component, OnInit } from '@angular/core';
import { ShippingChargeRequest, ShippingChargeResponse } from '../../interfaces/shipping';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ShippingChargeService } from '../../services/shipping-charge.service';
import { catchError, of, tap } from 'rxjs';
import { Observable } from 'rxjs'; 

@Component({
  selector: 'app-manage-shipping-charge',
  standalone: true, 
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule
  ],
  templateUrl: './manage-shipping-charge.component.html',
  styleUrl: './manage-shipping-charge.component.css'
})
export class ManageShippingChargeComponent implements OnInit{
  
  shippingChargeForm: FormGroup;
  
  shippingCharges: ShippingChargeResponse[] = [];
  
  editingCharge: ShippingChargeResponse | null = null;
  
  loading: boolean = false;
  
  error: string | null = null;

  constructor(
    private fb: FormBuilder, 
    private shippingChargeService: ShippingChargeService 
  ) {
    
    this.shippingChargeForm = this.fb.group({
      country: [null, Validators.required], 
      region: [null],
      productId: [null],
      minOrderAmount: [0, [Validators.required, Validators.min(0)]], 
      maxOrderAmount: [null],
      shippingFeePerKm: [0, [Validators.required, Validators.min(0)]], 
      isFree: [false],
      carrier: [null],
      
      effectiveFrom: [null],
      effectiveTo: [null]
    });
  }

  
  ngOnInit(): void {
    
    this.loadShippingCharges();
  }

  
  loadShippingCharges(): void {
    this.loading = true; 
    this.error = null; 
    
    
    
    this.shippingChargeService.getAll('someId')
      .pipe(
        
        tap(charges => {
          this.shippingCharges = charges; 
          this.loading = false; 
        }),
        
        catchError(err => {
          this.error = 'Failed to load shipping charges.'; 
          this.loading = false; 
          console.error('Error loading shipping charges:', err); 
          return of([]); 
        })
      )
      .subscribe(); 
  }

  
  onSubmit(): void {
    
    if (this.shippingChargeForm.invalid) {
      
      this.shippingChargeForm.markAllAsTouched();
      return; 
    }

    
    const formData: ShippingChargeRequest = { ...this.shippingChargeForm.value };

    
    
    
    if (formData.effectiveFrom) {
      try {
         
         const dateUTC = new Date(formData.effectiveFrom + 'T00:00:00Z');
         
         if (!isNaN(dateUTC.getTime())) {
            formData.effectiveFrom = dateUTC.toISOString();
         } else {
            console.error(`Invalid date value after appending T00:00:00Z: ${formData.effectiveFrom}`);
            this.error = '"Effective From" date is invalid.'; 
            return; 
         }
      } catch (e) {
         console.error("Error formatting effectiveFrom date:", e);
         this.error = 'Error processing "Effective From" date.'; 
         return; 
      }
    } else {
      
      formData.effectiveFrom = null;
    }

    if (formData.effectiveTo) {
       try {
          const dateUTC = new Date(formData.effectiveTo + 'T00:00:00Z');
          if (!isNaN(dateUTC.getTime())) {
             formData.effectiveTo = dateUTC.toISOString();
           } else {
             console.error(`Invalid date value after appending T00:00:00Z: ${formData.effectiveTo}`);
             this.error = '"Effective To" date is invalid.'; 
             return; 
           }
       } catch (e) {
          console.error("Error formatting effectiveTo date:", e);
          this.error = 'Error processing "Effective To" date.'; 
          return; 
       }
    } else {
       
       formData.effectiveTo = null;
    }
    

    if (this.editingCharge) {
      
      
      this.updateCharge(this.editingCharge.id, formData);
    } else {
      
      
      this.createCharge(formData);
    }
  }

  
  createCharge(charge: ShippingChargeRequest): void {
    this.loading = true; 
    this.error = null; 
    
    this.shippingChargeService.createShippingChargeEntry(charge)
      .pipe(
        
        tap(newCharge => {
          this.shippingCharges.push(newCharge); 
          this.resetForm(); 
          this.loading = false; 
        }),
        
        catchError(err => {
          this.error = 'Failed to create shipping charge.'; 
          this.loading = false; 
          console.error('Error creating shipping charge:', err); 
          
          
           return new Observable(subscriber => subscriber.error(err)); 
        })
      )
      .subscribe(); 
  }

  
  updateCharge(id: string, charge: ShippingChargeRequest): void {
    this.loading = true; 
    this.error = null; 
    
    this.shippingChargeService.updateShippingChargeEntry(id, charge)
      .pipe(
        
        tap(updatedCharge => {
          
          const index = this.shippingCharges.findIndex(c => c.id === updatedCharge.id);
          if (index !== -1) {
            
            this.shippingCharges[index] = updatedCharge;
          }
          this.resetForm(); 
          this.loading = false; 
        }),
        
        catchError(err => {
          this.error = 'Failed to update shipping charge.'; 
          this.loading = false; 
          console.error('Error updating shipping charge:', err); 
          
           return new Observable(subscriber => subscriber.error(err)); 
        })
      )
      .subscribe(); 
  }

  
  deleteCharge(id: string): void {
    
    if (confirm('Are you sure you want to delete this shipping charge?')) {
      this.loading = true; 
      this.error = null; 
      
      this.shippingChargeService.deleteEntry(id)
        .pipe(
          
          tap(() => {
            
            this.shippingCharges = this.shippingCharges.filter(charge => charge.id !== id);
            this.loading = false; 
          }),
          
          catchError(err => {
            this.error = 'Failed to delete shipping charge.'; 
            this.loading = false; 
            console.error('Error deleting shipping charge:', err); 
            
             return new Observable(subscriber => subscriber.error(err)); 
          })
        )
        .subscribe(); 
    }
  }

  
  editCharge(charge: ShippingChargeResponse): void {
    this.editingCharge = charge; 
    
    
    this.shippingChargeForm.patchValue({
      country: charge.country,
      region: charge.region,
      productId: charge.productId,
      minOrderAmount: charge.minOrderAmount,
      maxOrderAmount: charge.maxOrderAmount,
      shippingFeePerKm: charge.shippingFeePerKm,
      isFree: charge.isFree,
      carrier: charge.carrier,
      
      effectiveFrom: charge.effectiveFrom ? new Date(charge.effectiveFrom).toISOString().split('T')[0] : null,
      effectiveTo: charge.effectiveTo ? new Date(charge.effectiveTo).toISOString().split('T')[0] : null
    });
  }

  
  resetForm(): void {
    this.shippingChargeForm.reset({
      country: null,
      region: null,
      productId: null,
      minOrderAmount: 0,
      maxOrderAmount: null,
      shippingFeePerKm: 0,
      isFree: false,
      carrier: null,
      effectiveFrom: null,
      effectiveTo: null
    });
    this.editingCharge = null; 
     this.error = null; 
  }

  
  cancelEdit(): void {
    this.resetForm(); 
  }
}