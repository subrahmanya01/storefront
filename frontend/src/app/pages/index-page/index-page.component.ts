import { Component, OnInit } from '@angular/core';
import { CarouselComponent } from '../../building-blocks/carousel/carousel.component';
import { CarouselItem } from '../../interfaces/building-block/carousel-item';
import { LegendComponent } from '../../building-blocks/legend/legend.component';
import { ProductCardComponent } from '../../components/product-card/product-card.component';
import { ProductCarouselComponent } from '../../components/product-carousel/product-carousel.component';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { ProductInfoService } from '../../services/product-info.service';
import { SpinnerComponent } from '../../building-blocks/spinner/spinner.component';
import { ProductService } from '../../services/product.service';
import { RatingService } from '../../services/rating.service';
import { ExtendedProduct } from '../../interfaces/product-meta';
import { Router } from '@angular/router';
import { NormalItems } from '../../interfaces/component-models';
import { getCategoryNormalItems } from '../../helpers/helper';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-index-page',
  imports: [
    LegendComponent, 
    ProductCarouselComponent, 
    ButtonComponent, 
    CarouselComponent,
    SpinnerComponent
  ],
  templateUrl: './index-page.component.html',
  styleUrl: './index-page.component.css'
})
export class IndexPageComponent implements OnInit {
  isLoading: boolean =  false;
  bannerImages: CarouselItem[] = [];
  productCategories: string[] = [];

  flashSaleProducts: ExtendedProduct[] = [];
  productCategoryNormalItems: NormalItems[] = [];
  constructor(private readonly productInfoService: ProductInfoService,
    private readonly productService: ProductService,
    private readonly ratingService: RatingService,
    private readonly router: Router
  ){

  }
  ngOnInit(): void {
    this.isLoading = true;
    this.getBannerImages();
    this.getFlashSales();
    this.getCategories();
  }

  viewAllProducts()
  {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    this.router.navigateByUrl("search?key=");
  }

  getBannerImages()
  {
    this.productInfoService.getBannerImages().subscribe({
      next: (data: string[]) => {
        for(let i =0; i< data.length; i++)
        {
          this.bannerImages.push({
             imageUrl: data[i]
          } as any)
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  getFlashSales()
  {
    this.productInfoService.getFlashSaleProducts().subscribe({
      next: (data) => {
        this.flashSaleProducts = data;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  getCategories()
  {
    this.productInfoService.getProductCategories().subscribe({
      next: (data:string[]) => {
        this.productCategories = data;
        this.productCategoryNormalItems = getCategoryNormalItems(data);
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }
}
