import { Component, OnInit } from '@angular/core';
import { QuantitySelectorComponent } from '../../building-blocks/quantity-selector/quantity-selector.component';
import { RatingComponent } from '../../building-blocks/rating/rating.component';
import { ProductHeighlightComponent } from '../../components/product-heighlight/product-heighlight.component';
import { ColorSelectorComponent } from '../../building-blocks/color-selector/color-selector.component';
import { SizeSelectorComponent } from '../../building-blocks/size-selector/size-selector.component';
import { ProductViewerComponent } from '../../components/product-viewer/product-viewer.component';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { BreadScrumComponent } from '../../building-blocks/bread-scrum/bread-scrum.component';
import { LegendComponent } from '../../building-blocks/legend/legend.component';
import { ProductCarouselComponent } from '../../components/product-carousel/product-carousel.component';
import { ProductService } from '../../services/product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ExtendedProduct, Product, ProductsByIdsRequest, ProductVariant } from '../../interfaces/product-meta';
import { SnackbarService } from '../../services/snackbar.service';
import { CurrencyPipe } from '@angular/common';
import { RatingService } from '../../services/rating.service';
import { Rating } from '../../interfaces/rating';
import { ProductInfoService } from '../../services/product-info.service';
import { WatchlistService } from '../../services/watchlist.service';
import { AddWatchListRequest } from '../../interfaces/watchlist';
import { CartService } from '../../services/cart.service';
import { AddCartItemRequest } from '../../interfaces/cart-item';
import { ProductReviewsDisplayComponent } from '../../components/product-reviews-display/product-reviews-display.component';

@Component({
  selector: 'app-product-info',
  imports: [
    QuantitySelectorComponent, 
    RatingComponent, 
    ProductHeighlightComponent, 
    ColorSelectorComponent,
    SizeSelectorComponent, 
    ProductViewerComponent, 
    ButtonComponent, 
    BreadScrumComponent, 
    LegendComponent, 
    ProductCarouselComponent,
    ProductReviewsDisplayComponent
    ],
  templateUrl: './product-info.component.html',
  styleUrl: './product-info.component.css',
  providers:[
    CurrencyPipe
  ]
})
export class ProductInfoComponent implements OnInit {
  
  images: string[] = ["product-images/main.png", "product-images/side-1.png", "product-images/side-2.png", "product-images/side-3.png", "product-images/side-4.png"];
  sizes: string[] = ["XS", "S", "ML", "L", "XL"]
  
  productId: string = "";
  product: ExtendedProduct = {} as ExtendedProduct;
  selectedVariant: ProductVariant | undefined = {} as ProductVariant;
  productRating: Rating[] = [];
  productRatingOutof5: number = 0;
  filteredVarients: ProductVariant = {} as ProductVariant;
  relatedProducts: ExtendedProduct[] = [];
  currentAttributeSelection: Map<string, any> = new Map<string, string>();
  selectedQuantity: number = 1;
  isFreeDelevery: boolean = false;

  constructor(private readonly productService: ProductService, private readonly route: ActivatedRoute, 
    private readonly productInfoService:ProductInfoService,
    private readonly watchListService: WatchlistService,
    private readonly cartService: CartService,
    private router: Router,
    private readonly snackbar: SnackbarService, private readonly ratingService: RatingService)
  {
   
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.productId = params.get("id") ?? "";
      const req = { productIds: [this.productId]} as ProductsByIdsRequest;
      this.productInfoService.getProductsByIds(req).subscribe({
        next: (data: ExtendedProduct[])=>{
          this.product = data[0];
          console.log(this.product);
          this.productRatingOutof5 = this.product.rating;
          this.selectedVariant = this.product?.variants?.[0];
          this.getProductByCategory(this.product.category);
          this.isProductHaveFreeDelevery();
          if(this.selectedVariant)
          {
            for (const [key, value] of Object.entries(this.selectedVariant)) {
              this.currentAttributeSelection.set(key, value);
            }
          }
        },
        error: (err: any)=>{
          this.snackbar.error("Failed to load product");
        }
      })
    });

  }

  onQuantityChange(quantity: number)
  {
    this.selectedQuantity = quantity;
  }

  isProductHaveFreeDelevery()
  {
    this.isFreeDelevery = this.product.shippingCharges.filter(x=> x.isFree).length > 0;
  }


  addToWatchList()
  {
    const watchListRequestModel = {
      productId: this.product.id
    } as AddWatchListRequest;
    this.watchListService.addWatchList(watchListRequestModel).subscribe({
      next: (data) => {
        this.snackbar.success("Added to watch list successfully");
      },
      error: (err) => {
        this.snackbar.error("Failed - Add to watch list");
      }
    });
  }

  addToCart()
  {
    const addCartItemRequest = {
      productId: this.product.id,
      productVariantId: this.selectedVariant?.id,
      quantity: this.selectedQuantity
    } as AddCartItemRequest;

    this.cartService.addCartItem(addCartItemRequest).subscribe({
      next: (data : any) => {
        this.router.navigateByUrl("/cart")
      },
      error: (err) => {
        console.error(err.message)
        this.snackbar.error("Failed - Add to cart");
      }
    });
  }

  getProductImages()
  {
    let images = this.selectedVariant?.images??["https://placehold.co/60x60/cccccc/333333?text=N/A"];
    if(images.length <=1)
    {
      images.push("https://placehold.co/60x60/cccccc/333333?text=N/A")
    }
    return images;
  }



  onCombinationSelection(data:any, attribute: string)
  {
    let prevMap = this.currentAttributeSelection;
    this.currentAttributeSelection.set(attribute, data);

    const varient = this.getMatchingVarient();

    if(!varient)
    {
      this.currentAttributeSelection = prevMap;
      this.snackbar.error("Product combination not found");
      return;
    }
    this.selectedVariant = varient;
  }

  getMatchingVarient()
  {
    for(let item of this.product.variants??[])
    {
      let cnt = 0;
      for(let key of this.currentAttributeSelection.keys())
      {
        if(item.attributes[key] == this.currentAttributeSelection.get(key))
        {
          cnt++;
        }
      }
      if(cnt == this.currentAttributeSelection.size)
      {
        return item;
      }
    }
    return null;
  }

  getProductByCategory(category: string)
  {
    this.productInfoService.getProductByCategories(category).subscribe({
      next: (data) => {
        this.relatedProducts = data;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }


  getAttributeValues(attribute:string):string[]
  {
    return [...(this.product.attributeValues?.[attribute] ?? [])];
  }

  getRelavantAttributeValuesForSelectedAttribute(attributeName: string, attributeValue: string)
  {
    const variants = [];
    for(let item of this.product.variants??[])
    {
      if(item.attributes[attributeName] == attributeValue)
      {
        variants.push(item);
      }
    }
  }
}
