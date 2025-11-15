import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { RatingService } from '../../services/rating.service'; 
import { Rating } from '../../interfaces/rating';
import { RatingComponent } from '../../building-blocks/rating/rating.component';
import { LegendComponent } from '../../building-blocks/legend/legend.component';

@Component({
  selector: 'app-product-reviews-display',
  standalone: true,
  imports: [
    CommonModule,
    RatingComponent,
    LegendComponent
  ],
  templateUrl: './product-reviews-display.component.html',
  styleUrl: './product-reviews-display.component.css'
})
export class ProductReviewsDisplayComponent implements OnInit {
  @Input() productId!: string; 
  productReviews: Rating[] = [];
  loadingReviews = false;
  errorMessage = '';

  constructor(private readonly ratingService: RatingService) { }

  ngOnInit(): void {
    this.loadProductReviews();
  }

  loadProductReviews(): void {
    if (this.productId) {
      this.loadingReviews = true;
      this.ratingService.getRating(this.productId).subscribe({
        next: (reviews) => {
          this.productReviews = reviews;
        },
        error: (err) => {
          console.error('Error loading product reviews:', err);
          this.errorMessage = 'Failed to load reviews for this product.';
        },
        complete: () => {
          this.loadingReviews = false;
        }
      });
    } else {
      console.warn('Product ID not provided to ProductReviewsDisplayComponent.');
      this.errorMessage = 'Product information is missing.';
    }
  }

  getStarArray(count: number): number[] {
    return Array(count).fill(0).map((_, index) => index + 1);
  }
}