import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BreadScrumComponent } from '../../building-blocks/bread-scrum/bread-scrum.component';
import { OrderService } from '../../services/order.service';
import { ExtendedProduct, ProductsByIdsRequest } from '../../interfaces/product-meta';
import { OrderResponse } from '../../interfaces/order';
import { ProductInfoService } from '../../services/product-info.service';
import { RatingService } from '../../services/rating.service'; 
import { ProductRatingComponent } from '../../components/product-rating/product-rating.component';
import { Rating } from '../../interfaces/rating';
import { SpinnerComponent } from '../../building-blocks/spinner/spinner.component';

@Component({
  selector: 'app-my-reviews',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    BreadScrumComponent,
    SpinnerComponent,
    ProductRatingComponent 
  ],
  templateUrl: './my-reviews.component.html',
  styleUrl: './my-reviews.component.css'
})
export class MyReviewsComponent implements OnInit {
  isLoading: boolean = false;
  orderedProducts: ExtendedProduct[] = [];
  userRatings: { [productId: string]: Rating | null } = {};
  loadingOrders = false;
  loadingProducts = false;
  loadingRatings = false;
  errorMessage = '';

  constructor(
    private readonly orderService: OrderService,
    private readonly productInfoService: ProductInfoService,
    private readonly ratingService: RatingService 
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.loadOrdersAndProducts();
    this.loadUserRatings();
    setTimeout(()=>{
      this.isLoading = false;
    }, 1000);
  }

  loadOrdersAndProducts(): void {
    this.loadingOrders = true;
    this.orderService.getOrderByCustomerId().subscribe({
      next: (data: OrderResponse[]) => {
        const productIds: string[] = [];
        for (const order of data) {
          for (const item of order.orderItems) {
            productIds.push(item.productId);
          }
        }
        const uniqueProductIds = [...new Set(productIds)];
        this.getProductsById(uniqueProductIds);
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
        this.errorMessage = 'Failed to load your order history.';
        this.loadingOrders = false;
      },
      complete: () => {
        this.loadingOrders = false;
      }
    });
  }

  getProductsById(productIds: string[]): void {
    this.loadingProducts = true;
    const request = {
      productIds: productIds
    } as ProductsByIdsRequest;
    this.productInfoService.getProductsByIds(request).subscribe({
      next: (data) => {
        this.orderedProducts = data;
      },
      error: (err) => {
        console.error('Error fetching product details:', err);
        this.errorMessage = 'Failed to load product details.';
        this.loadingProducts = false;
      },
      complete: () => {
        this.loadingProducts = false;
      }
    });
  }

  loadUserRatings(): void {
    this.loadingRatings = true;
    this.ratingService.getUserRatings().subscribe({
      next: (ratings) => {
        this.userRatings = ratings.reduce((acc, rating) => {
          acc[rating.productId] = rating;

          return acc;
        }, {} as { [productId: string]: Rating | null });
      },
      error: (err) => {
        console.error('Error fetching user ratings:', err);
        this.errorMessage = 'Failed to load your existing ratings.';
        this.loadingRatings = false;

      },
      complete: () => {
        this.loadingRatings = false;
      }
    });
  }

  trackProductById(index: number, product: ExtendedProduct): string {
    return product.id;
  }
}