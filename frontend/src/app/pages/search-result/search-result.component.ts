import { Component } from '@angular/core';
import { SearchResultCardComponent } from '../../components/search-result-card/search-result-card.component';
import { CommonModule } from '@angular/common';
import { Product } from '../../interfaces/product-meta';
import { ProductService } from '../../services/product.service';
import { SearchFilters, SearchRequest } from '../../interfaces/search';
import { ActivatedRoute } from '@angular/router';
import { SearchFilterComponent } from '../../components/search-filter/search-filter.component';
import { ProductInfoService } from '../../services/product-info.service';
import { SpinnerComponent } from '../../building-blocks/spinner/spinner.component';

@Component({
  selector: 'app-search-result',
  imports: [
    SearchResultCardComponent,
    CommonModule,
    SearchFilterComponent,
    SpinnerComponent
  ],
  templateUrl: './search-result.component.html',
  styleUrl: './search-result.component.css'
})
export class SearchResultComponent {
  isLoading: boolean = false;
  products: Product[] = [];
  brands:string[] = [];
  categories: string[] = [];
  searchKeyword: string = "";

  constructor(private readonly productService: ProductService, 
    private readonly productInfoService: ProductInfoService,
    private readonly route: ActivatedRoute) { }

  ngOnInit(): void {
    this.isLoading =true;
    this.route.queryParams.subscribe(params => {
      const keyword = params['key'];
      this.searchKeyword = keyword;
      this.productService.searchResults({keyword: keyword} as SearchRequest).subscribe({
        next: (data: Product[])=>{
          this.products = data;
          this.getCategories();
          this.getBrands();
        }
      })
    });
    
  }

  onFilterChange(val: SearchFilters)
  {
    val.brand = val.brand == "null"? "":val.brand;
    val.category = val.category == "null"? "":val.category;

    this.productService.searchResults({
      keyword: this.searchKeyword,
      filters: val
    } as SearchRequest).subscribe({
      next: (data: Product[])=>{
        this.products = data;
      }
    })
  }

  getCategories()
  {
    this.productInfoService.getProductCategories().subscribe({
      next: (data) => {
        this.categories = data.filter(x=> x.trim().length > 0);
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  getBrands()
  {
    this.productInfoService.getProductBrands().subscribe({
      next: (data) => {
        this.brands = data.filter(x=> x.trim().length > 0);
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        console.error(err.message);
      }
    });
  }
}
