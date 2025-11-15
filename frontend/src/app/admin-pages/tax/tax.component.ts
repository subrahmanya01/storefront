import { Component } from '@angular/core';
import { catchError, of, tap } from 'rxjs';
import { TaxRateRequest, TaxRateResponse } from '../../interfaces/tax';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaxService } from '../../services/tax.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tax',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './tax.component.html',
  styleUrl: './tax.component.css'
})
export class TaxComponent {
  taxRateForm: FormGroup;
  
  taxRates: TaxRateResponse[] = [];
  
  selectedTaxRate: TaxRateResponse | null = null;
  
  isLoading: boolean = false;
  
  errorMessage: string | null = null;
  
  successMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private taxService: TaxService
  ) {
    
    this.taxRateForm = this.fb.group({
      country: [null, Validators.required],
      state: [null], 
      rate: [null, [Validators.required, Validators.min(0)]],
      category: [null] 
    });
  }

  ngOnInit(): void {
    
    this.getAllTaxRates();
  }

  /**
   * Handles form submission for creating or updating a tax rate.
   */
  onSubmit(): void {
    
    if (this.taxRateForm.invalid) {
      this.errorMessage = 'Please fill in all required fields.';
      this.successMessage = null;
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;
    const formValue: TaxRateRequest = this.taxRateForm.value;

    
    if (this.selectedTaxRate) {
      this.taxService.update(this.selectedTaxRate.id, formValue).pipe(
        tap(() => {
          this.successMessage = 'Tax rate updated successfully!';
          this.resetForm();
          this.getAllTaxRates(); 
        }),
        catchError(error => {
          this.errorMessage = 'Error updating tax rate.';
          console.error('Update error:', error);
          return of(null);
        })
      ).subscribe(() => this.isLoading = false);
    } else {
      
      this.taxService.create(formValue).pipe(
        tap(() => {
          this.successMessage = 'Tax rate created successfully!';
          this.resetForm();
          this.getAllTaxRates(); 
        }),
        catchError(error => {
          this.errorMessage = 'Error creating tax rate.';
          console.error('Create error:', error);
          return of(null);
        })
      ).subscribe(() => this.isLoading = false);
    }
  }

  /**
   * Retrieves all tax rates from the service.
   */
  getAllTaxRates(): void {
    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;
    this.taxService.getAll().pipe(
      tap(rates => {
        this.taxRates = rates;
      }),
      catchError(error => {
        this.errorMessage = 'Error fetching tax rates.';
        console.error('Get all error:', error);
        this.taxRates = []; 
        return of([]);
      })
    ).subscribe(() => this.isLoading = false);
  }

  /**
   * Selects a tax rate for editing.
   * @param rate The tax rate to select.
   */
  selectTaxRateForEdit(rate: TaxRateResponse): void {
    this.selectedTaxRate = rate;
    
    this.taxRateForm.patchValue({
      country: rate.country,
      state: rate.state,
      rate: rate.rate,
      category: rate.category
    });
    this.errorMessage = null;
    this.successMessage = null;
  }

  /**
   * Deletes a tax rate by its ID.
   * @param id The ID of the tax rate to delete.
   */
  deleteTaxRate(id: string): void {
    if (confirm('Are you sure you want to delete this tax rate?')) {
      this.isLoading = true;
      this.errorMessage = null;
      this.successMessage = null;
      this.taxService.delete(id).pipe(
        tap(success => {
          if (success) {
            this.successMessage = 'Tax rate deleted successfully!';
            this.getAllTaxRates(); 
            if (this.selectedTaxRate?.id === id) {
              this.resetForm(); 
            }
          } else {
             this.errorMessage = 'Failed to delete tax rate.';
          }
        }),
        catchError(error => {
          this.errorMessage = 'Error deleting tax rate.';
          console.error('Delete error:', error);
          return of(false);
        })
      ).subscribe(() => this.isLoading = false);
    }
  }

  /**
   * Resets the form and clears the selected tax rate.
   */
  resetForm(): void {
    this.taxRateForm.reset();
    this.selectedTaxRate = null;
    this.errorMessage = null;
    this.successMessage = null;
  }
}
