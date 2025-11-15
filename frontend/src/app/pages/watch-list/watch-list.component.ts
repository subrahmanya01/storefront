import { Component, OnInit } from '@angular/core';
import { ProductCarouselComponent } from '../../components/product-carousel/product-carousel.component';
import { LegendComponent } from '../../building-blocks/legend/legend.component';
import { ExtendedProduct, ProductsByIdsRequest } from '../../interfaces/product-meta';
import { WatchlistService } from '../../services/watchlist.service';
import { RemoveWatchListItemRequest, WatchListResponse } from '../../interfaces/watchlist';
import { ProductInfoService } from '../../services/product-info.service';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { SnackbarService } from '../../services/snackbar.service';
import { SpinnerComponent } from '../../building-blocks/spinner/spinner.component';
import { SharedDataAccessService } from '../../services/shared-data-access.service';

@Component({
  selector: 'app-watch-list',
  imports: [
    ProductCarouselComponent,
    LegendComponent,
    ButtonComponent,
    SpinnerComponent
    ],
  templateUrl: './watch-list.component.html',
  styleUrl: './watch-list.component.css'
})
export class WatchListComponent implements OnInit {

  products: ExtendedProduct[] = [];
  watchList: WatchListResponse[] = [];
  isLoading: boolean = false;

  constructor(private readonly watchListService: WatchlistService, 
    private readonly snackbar: SnackbarService,
    private readonly sharedDataAccess: SharedDataAccessService,
    private readonly productInfoService: ProductInfoService){
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.getWatchListItems();
    this.sharedDataAccess.onWatchlistRemove$.subscribe({
      next: (data: string)=>{
        if(data)
        {
          this.removeItemFromWatchList(data);
        }
      },
      error:()=>{
        this.snackbar.error("Failed - To remove item from watch list");
      }
    })
  }

  removeItemFromWatchList(productId: string)
  {
    const request = { productId: productId} as RemoveWatchListItemRequest;
    this.watchListService.removeWatchListItem(request).subscribe({
      next: (data) => {
        this.snackbar.success("Success - Item removed from watch list");
        this.getWatchListItems();
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  getWatchListItems()
  {
    this.watchListService.getWatchListItems().subscribe({
      next: (data) => {
        this.watchList = data;
        if(!this.watchList || this.watchList.length ==0)
        {
          this.isLoading = false;
          return;
        }
        const ids = this.watchList.map(item => item.productId).filter(item => !(item === null));
        this.getProducts(ids);
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  getProducts(ids:string[])
  {
    this.productInfoService.getProductsByIds({ productIds: [...ids]} as ProductsByIdsRequest).subscribe({
      next: (data: ExtendedProduct[]) => {
        this.products = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  clearWatchList()
  {
    this.watchListService.clearWatchList().subscribe({
      next: (data) => {
        this.products =[];
        this.snackbar.success("Watch list cleared successfully");
      },
      error: (err) => {
        this.snackbar.error("Failed to clear watchlist")
      }
    });
  }

}
