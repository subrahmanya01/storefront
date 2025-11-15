import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product, ProductVariant } from '../../interfaces/product-meta';
import { ProductService } from '../../services/product.service';
import { ProductFormComponent } from '../components/product-form/product-form.component';
import { ProductVariantListComponent } from '../components/product-variant-list/product-variant-list.component';
import { ActivatedRoute } from '@angular/router';



@Component({
  selector: 'app-manage-product',
  imports: [
    CommonModule,
    ProductFormComponent,
    ProductVariantListComponent
  ],
  templateUrl: './manage-product.component.html',
  styleUrl: './manage-product.component.css'
})
export class ManageProductComponent implements OnInit{
  activeTab: 'product' | 'variants' = 'product';
  editingProduct: Product | null = null;
  currentProductId: string = "";
  constructor(private productService: ProductService, private readonly activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    const productId = this.activatedRoute.snapshot.paramMap.get('id');
    this.currentProductId = productId ?? "";
    if(productId)
    { 
      this.getProduct(productId);
    }
  }

  getProduct(productId: string)
  {
    this.productService.getProduct(productId).subscribe({
      next: (data:Product)=>{
        this.editingProduct = data;
      }
    })
  }

  setActiveTab(tab: 'product' | 'variants'): void {
    this.activeTab = tab;
  }

  onProductSaved(product: Product): void {
    this.getProduct(product.id);
    
    if (product.id) {
      this.setActiveTab('variants');
    }
    
  }

  onVariantsUpdated(updatedVariants: any): void {
    if (this.editingProduct) {
      this.getProduct(this.editingProduct.id);
    }
  }
}
