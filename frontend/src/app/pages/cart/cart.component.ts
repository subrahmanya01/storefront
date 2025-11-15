import { Component, OnInit } from '@angular/core';
import { BreadScrumComponent } from '../../building-blocks/bread-scrum/bread-scrum.component';
import { CartListComponent } from '../../components/cart-list/cart-list.component';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { CartService } from '../../services/cart.service';
import { ProductService } from '../../services/product.service';
import { ProductInfoService } from '../../services/product-info.service';
import { CartItem, CartItemResponse, CartResponse, UpdateQuantityRequest } from '../../interfaces/cart-item';
import { ExtendedProduct, ProductsByIdsRequest } from '../../interfaces/product-meta';
import { SpinnerComponent } from '../../building-blocks/spinner/spinner.component';
import { SnackbarService } from '../../services/snackbar.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  imports: [
    BreadScrumComponent, 
    CartListComponent, 
    ButtonComponent,
    SpinnerComponent
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit{
  cartInfo: CartResponse = {} as CartResponse;
  productsInfo: ExtendedProduct[] = []
  mappedCartItems: CartItem[] = [];
  isLoading: boolean = false;

  totalCartAmount: number = 0;
  isNoCartItems: boolean = false;
  updatedCart: CartItem[] = [];

  constructor(private readonly cartService: CartService,
    private readonly snackbar: SnackbarService,
    private readonly router: Router,
    private readonly productInfoService: ProductInfoService
  )
  {

  }
  ngOnInit(): void {
    this.isLoading = true;
    this.getCartItems();
  }

  onDeleteEvent(cartId:string)
  {
    this.deleteCartItem(cartId);
  }

  onQuantityUpdateEvent(item: CartItem)
  {
    this.updateQuantity(item.cartId, item.quantity);
  }

  getCartItems()
  {
    this.cartService.getCartItems().subscribe({
      next: (data: CartResponse) => {
        this.cartService.cartInfo.next(data);
        this.cartInfo = data;
        if(!this.cartInfo)
        {
          this.isNoCartItems = true;
          this.isLoading = false;
          return;
        }
        const productIds = this.cartInfo?.items.map((x:CartItemResponse)=> x.productId);
        if(productIds.length == 0)
        {
          this.isNoCartItems = true;
          this.isLoading = false;
          return;
        }
        const request = {productIds: productIds} as ProductsByIdsRequest;
        this.productInfoService.getProductsByIds(request).subscribe({
          next: (data: ExtendedProduct[])=>{
            this.createCartItemModel(this.cartInfo.items, data);
            this.isLoading = false;
          }
        })
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  createCartItemModel(cartItems: CartItemResponse[], products: ExtendedProduct[])
  {
    const newCartItems: CartItem[] = []
    for(let item of cartItems)
    {
      const product = products.find((x:ExtendedProduct)=> x.id == item.productId);
      if(product)
      {
        const variantInfo = product.variants?.find(x=> x.id == item.variantId);
        if(variantInfo)
        {
          newCartItems.push({
            cartId: item.id,
            name: product.name,
            price: variantInfo.price,
            quantity: item.quantity,
            imageUrl: variantInfo.images[0],
            productData: product
          } as CartItem)
        }
      }
    }
    this.mappedCartItems = newCartItems;
    this.updatedCart = this.mappedCartItems;
    this.totalCartAmount = this.updatedCart.map(x=> x.price* x.quantity).reduce((acc, curr) => acc + curr, 0)
  }

  updateQuantity(cartItemId: string, quantity: number)
  {
    const request = {quantity: quantity} as UpdateQuantityRequest;
    this.cartService.updateQuantity(cartItemId, request).subscribe({
      next: (data) => {
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  onCartUpdate(items: CartItem[])
  {
    this.updatedCart = items;
    this.totalCartAmount = this.updatedCart.map(x=> x.price* x.quantity).reduce((acc, curr) => acc + curr, 0)
  }

  deleteCartItem(itemId: string)
  {
    this.cartService.deleteCartItem(itemId).subscribe({
      next: (data) => {
        this.snackbar.success("Item removed from cart successfully");
        this.getCartItems();
      },
      error: (err) => {
        this.snackbar.error("Failed to remove item from cart");
      }
    });
  }

  onCheckout()
  {
    this.router.navigateByUrl("/checkout")
  }

}
