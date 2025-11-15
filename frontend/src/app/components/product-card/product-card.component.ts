import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RatingComponent } from '../../building-blocks/rating/rating.component';
import { ProductMeta } from '../../interfaces/product-meta';
import { Router } from '@angular/router';
import { AddWatchListRequest } from '../../interfaces/watchlist';
import { WatchlistService } from '../../services/watchlist.service';
import { SnackbarService } from '../../services/snackbar.service';
import { CartService } from '../../services/cart.service';
import { AddCartItemRequest } from '../../interfaces/cart-item';
import { SharedDataAccessService } from '../../services/shared-data-access.service';

@Component({
  selector: 'app-product-card',
  imports: [RatingComponent],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css'
})
export class ProductCardComponent implements OnInit {
  @Input() productInfo: ProductMeta = {} as any;
  @Input() showDeleteIcon: boolean = false;
  @Output() onAddtoCartClick: any = new EventEmitter<any>();
  rating: number = 3;


  constructor(private readonly router: Router, 
    private readonly watchListService: WatchlistService,
    private readonly cartService: CartService,
    private readonly sharedDataAccessService: SharedDataAccessService,
    private readonly snackbar: SnackbarService) {
  }

  ngOnInit(): void {

  }

  onAddToCart()
  {
    this.onAddtoCartClick.emit(this.productInfo.id);
  }

  addToWatchList() {
    const watchListRequestModel = {
      productId: this.productInfo.id
    } as any;
    this.watchListService.addWatchList(watchListRequestModel).subscribe({
      next: (data) => {
        this.snackbar.success("Added to watch list successfully");
      },
      error: (err) => {
        this.snackbar.error("Failed - Add to watch list");
      }
    });
  }

  removeFromWatchList()
  {
    this.sharedDataAccessService.setWatchListRemoveItem(this.productInfo.id);
  }

  addToCart()
  {
    const addCartItemRequest = {
      productId: this.productInfo.id,
      productVariantId: this.productInfo.variantId,
      quantity: 1
    } as any;

    this.cartService.addCartItem(addCartItemRequest).subscribe({
      next: (data : any) => {
        this.router.navigateByUrl("/cart")
        window.scrollTo(0,0);
      },
      error: (err) => {
        console.error(err.message)
        this.snackbar.error("Failed - Add to cart");
      }
    });
  }

  onRatingChange(val: number) {
    this.rating = val;
  }

  getProperPrice(product: ProductMeta) {
    return product.discount != 0 ? product.discountedPrice : product.actualPrice;
  }

  navigateToProductInfo() {
    this.router.navigateByUrl(`product/${this.productInfo.id}`);
    window.scrollTo(0, 0);
  }
}
