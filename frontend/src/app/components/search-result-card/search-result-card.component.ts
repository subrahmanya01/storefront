import { Component, Input } from '@angular/core';
import { Product } from '../../interfaces/product-meta';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-search-result-card',
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './search-result-card.component.html',
  styleUrl: './search-result-card.component.css',
  providers:[
    CurrencyPipe
  ]
})
export class SearchResultCardComponent {
  @Input() product!: Product;

  constructor() { }

  getFirstImageUrl(): string | null {
    if (this.product && this.product.variants && this.product.variants.length > 0 && this.product.variants[0].images && this.product.variants[0].images.length > 0) {
      return this.product.variants[0].images[0];
    }
    return null; 
  }

  getFirstVariantPrice(): number | null {
    if (this.product && this.product.variants && this.product.variants.length > 0) {
      return this.product.variants[0].price;
    }
    return null;
  }
}
