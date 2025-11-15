import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { RatingService } from '../../services/rating.service'; 
import { ExtendedProduct } from '../../interfaces/product-meta'; 
import { Rating, RatingRequest } from '../../interfaces/rating';

@Component({
  selector: 'app-product-rating',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './product-rating.component.html',
  styleUrl: './product-rating.component.css'
})
export class ProductRatingComponent implements OnInit {
  @Input() product!: ExtendedProduct;

  userRating: Rating | null = null;
  showAddReviewForm = false;
  ratingForm: any;
  errorMessage: string = '';
  isDeleting = false;
  isSubmitting = false;

  constructor(
    private readonly ratingService: RatingService,
    private readonly fb: FormBuilder,
    private cdk: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadUserRating();
    this.ratingForm = this.fb.group({
      stars: [0, Validators.required],
      comment: ['']
    });
  }

  loadUserRating(): void {
    if (this.product && this.product.id) {
      this.ratingService.getUserRatings().subscribe({
        next: (ratings) => {
          console.log("user-rating", ratings)
          this.userRating = ratings.find(r => r.productId === this.product.id) || null;
          
          if (this.userRating) {
            this.ratingForm.patchValue({
              stars: this.userRating.stars,
              comment: this.userRating.comment
            });
          }
        },
        error: (err) => {
          console.error('Error loading user rating:', err);
          this.errorMessage = 'Failed to load your review.';
        }
      });
    }
  }

  toggleAddReviewForm(): void {
    this.showAddReviewForm = !this.showAddReviewForm;
    this.ratingForm.reset({ stars: 0, comment: '' }); 
    this.errorMessage = '';
  }

  submitRating(): void {
    if (this.ratingForm.valid && this.product && this.product.id && !this.isSubmitting) {
      this.isSubmitting = true;
      const ratingRequest: RatingRequest = {
        productId: this.product.id,
        rating: this.ratingForm.get('stars')!.value!,
        comment: this.ratingForm.get('comment')!.value
      };

      this.ratingService.addRating(ratingRequest).subscribe({
        next: (newRating) => {
          this.userRating = newRating;
          this.showAddReviewForm = false;
          this.errorMessage = '';
        },
        error: (err) => {
          console.error('Error adding rating:', err);
          this.errorMessage = 'Failed to add your review.';
        },
        complete: () => {
          this.isSubmitting = false;
        }
      });
    } else if (this.isSubmitting) {
      this.errorMessage = 'Please wait, submitting your review...';
    } else {
      this.errorMessage = 'Please select a rating.';
    }
  }

  deleteRating(): void {
    if (this.userRating && this.userRating.id && !this.isDeleting) {
      this.isDeleting = true;
      this.ratingService.deleteRating(this.userRating.id).subscribe({
        next: (success) => {
          this.userRating = null;
            this.errorMessage = '';
            this.cdk.detectChanges();
        },
        error: (err) => {
          console.error('Error deleting rating:', err);
          this.errorMessage = 'Failed to delete your review.';
        },
        complete: () => {
          this.isDeleting = false;
        }
      });
    } else if (this.isDeleting) {
      this.errorMessage = 'Please wait, deleting your review...';
    }
  }

  getStarArray(count: number): number[] {
    return Array(count).fill(0).map((_, index) => index + 1);
  }
}