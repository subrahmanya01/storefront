import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FlashSale, FlashSaleRequestModel } from '../../../interfaces/flashsales';
import { Subject } from 'rxjs';
import { ProductService } from '../../../services/product.service';
import { ExtendedProduct, Product } from '../../../interfaces/product-meta';
import { SpinnerComponent } from '../../../building-blocks/spinner/spinner.component';
import { SnackbarService } from '../../../services/snackbar.service';
import { ProductInfoService } from '../../../services/product-info.service';
interface GroupedSaleDisplay {
  startsAt: string;
  endsAt: string;
  products: {
    id: string; 
    productId: string | null;
    name: string;
  }[];
}
@Component({
  selector: 'app-flash-sales',
  imports: [
    CommonModule,
    FormsModule,
    SpinnerComponent
  ],
  templateUrl: './flash-sales.component.html',
  styleUrl: './flash-sales.component.css'
})
export class FlashSalesComponent implements OnInit {
  availableProducts: any[] = [];
  isLoading: boolean = false;
  productSearchTerm: string = '';
  filteredProducts: any[] = [];
  showSuggestions: boolean = false; 
  selectedProductsForNewSale: any[] = [];
  newSaleStartDate: string = ''; 
  newSaleEndDate: string = ''; 
  flashSaleProductList: ExtendedProduct[] = [];
  currentDateTime: string; 
  flashSaleList: FlashSale[] = []
  private destroy$ = new Subject<void>();

  constructor(private productService: ProductService, private productInfoService: ProductInfoService, private snackbar: SnackbarService) {
     const now = new Date();
     now.setMinutes(now.getMinutes() - now.getTimezoneOffset()); 
     this.currentDateTime = now.toISOString().slice(0, 16);
   }

  ngOnInit(): void {
    this.getProducts();
    this.getAllFlashSaleItems();
    this.filterProducts();
  }
  
  getProducts()
  {
    this.productService.getProducts().subscribe({
      next:(data: Product[])=>{
        this.availableProducts = data;
      }
    })
  }
  
  getAllFlashSaleItems()
  {
    this.productInfoService.getAllFlashSaleList().subscribe({
      next: (data) => {
        this.flashSaleList = data;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  trackById(index: number, item: any): any {
    return item.id; 
  }

  onProductSearch(): void {
    this.filterProducts();
    this.showSuggestions = true;
  }

  filterProducts(): void {
    const term = this.productSearchTerm.toLowerCase().trim();
    if (!term) {
      this.filteredProducts = []; 
    } else {
      this.filteredProducts = this.availableProducts.filter(product =>
        product.name.toLowerCase().includes(term) &&
        !this.selectedProductsForNewSale.some(p => p.id === product.id) 
      );
    }
  }

  selectProductSuggestion(product: any): void {
    if (!this.selectedProductsForNewSale.find(p => p.id === product.id)) {
      this.selectedProductsForNewSale.push(product);
    }
    this.productSearchTerm = '';
    this.filteredProducts = [];
    this.showSuggestions = false;
  }

  hideSuggestions(): void {
    setTimeout(() => {
      this.showSuggestions = false;
      this.filteredProducts = []; 
    }, 100);
  }

  removeProductFromNewSale(productId: string): void {
    this.selectedProductsForNewSale = this.selectedProductsForNewSale.filter(p => p.id !== productId);
    this.filterProducts();
  }

  addFlashSale(): void {
    if (this.selectedProductsForNewSale.length === 0) {
      this.snackbar.error('Please select at least one product.');
      return;
    }
    if (!this.newSaleStartDate || !this.newSaleEndDate) {
      this.snackbar.error('Please select both start and end dates.');
      return;
    }

    if (new Date(this.newSaleEndDate).getTime() <= new Date(this.newSaleStartDate).getTime()) {
        this.snackbar.error('End date must be after start date.');
        return;
    }

    const newEntries: FlashSaleRequestModel[] = this.selectedProductsForNewSale.map(product => ({
        productId: product.id,
        startsAt: this.newSaleStartDate,
        endsAt: this.newSaleEndDate
    }));
    
    this.productInfoService.addFlashSales(newEntries).subscribe({
      next: (data) => {
        this.snackbar.success("Flash sales added successfully")
        this.selectedProductsForNewSale = [];
        this.productSearchTerm = '';
        this.filteredProducts = [];
        this.showSuggestions = false;
        this.newSaleStartDate = '';
        this.newSaleEndDate = '';
        window.location.reload();
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  get groupedFlashSales(): GroupedSaleDisplay[] {
      const groupsMap = new Map<string, GroupedSaleDisplay>();
      
      this.flashSaleList.forEach((sale: FlashSale) => {
          const key = `${sale.startsAt}|${sale.endsAt}`; 

          if (!groupsMap.has(key)) {
              groupsMap.set(key, {
                  startsAt: sale.startsAt,
                  endsAt: sale.endsAt,
                  products: [],
              });
          }

          const productInfo = this.getProductInfo(sale.productId??"");

          if(productInfo)
          {
            groupsMap.get(key)!.products.push({
              id: sale.id, 
              productId: sale.productId,
              name: productInfo.name,
          } as any);
          }
      });

      const groupedArray = Array.from(groupsMap.values());
      groupedArray.sort((a, b) => new Date(a.startsAt).getTime() - new Date(b.startsAt).getTime());

      return groupedArray;
  }

  getProductInfo(productId: string)
  {
    return this.availableProducts.find((item: ExtendedProduct)=> item.id == productId);
  }

  removeFlashSaleItem(itemId: string): void {
      const initialLength = this.flashSaleProductList.length;
      this.flashSaleList = this.flashSaleList.filter((item:any) => item.id !== itemId);
      this.productInfoService.deleteFlashSale(itemId).subscribe({
        next: (data) => {

          this.snackbar.info("Deleted successfully")
        },
        error: (err) => {
          this.snackbar.error("Failed to delete")
        }
      });
  }

  getProductName(productId: string | null): string {
    if (!productId) return 'Unknown Product';
    const product = this.availableProducts.find(p => p.id === productId);
    return product ? product.name : `ID: ${productId}`;
  }

  formatDateDisplay(dateString: string): string {
    if (!dateString) return '';
    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      hour12: true 
    };
    try {
      return new Date(dateString).toLocaleString('en-US', options);
    } catch (e) {
      return 'Invalid Date';
    }
  }

  trackByIndex(index: number, item: any): number {
    return index;
  }
  
   trackByItemId(index: number, item: { id: string }): string {
       return item.id;
   }
}
