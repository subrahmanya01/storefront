import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product.service';
import { Product } from '../../interfaces/product-meta';
import { Router } from '@angular/router';
import { ButtonComponent } from '../../building-blocks/button/button.component';

@Component({
  selector: 'app-home',
  imports: [
    CommonModule,
    ButtonComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  products: Product[] = [];

  constructor(private readonly productService: ProductService,
    private readonly router: Router
  ) { }

  ngOnInit(): void {
    this.products = [];
    this.getAllProducts();
  }
  
  getAllProducts()
  {
    this.productService.getProducts().subscribe({
      next: (product: Product[])=>{
        this.products = product;
      },
      error: (err: any)=>{
        console.error(err);
      }
    })
  }
  addProduct()
  {
    this.router.navigateByUrl(`/admin/product/new`)
  }
  editProduct(product: Product): void {
    this.router.navigateByUrl(`/admin/product/${product.id}`)
  }

  getImageUrl(name: string)
  {
    if(name.trim().length  ==0)
      return 'https://placehold.co/60x60/cccccc/333333?text=N/A';
    else
    {
      return `https://placehold.co/60x60/cccccc/333333?text=${name.substring(0, 7)}`;
    }
  }

  getProductStock(product: Product)
  {
    let stock = 0;
    for(let variant of product.variants??[])
    {
      stock+=variant.inventory;
    }
    return stock;
  }

  getProductPrice(product: Product)
  {
    return (product.variants??[]).length > 0 ? product.variants?.[0].price : 0
  }

  deleteProduct(productId: string): void {
    this.productService.deleteProduct(productId).subscribe({
      next:()=>{
        this.getAllProducts();
      }
    });
  }

  handleImageError(event: Event): void {
    const element = event.target as HTMLImageElement;
    element.src = 'https://placehold.co/60x60/cccccc/333333?text=N/A';
  }
}
