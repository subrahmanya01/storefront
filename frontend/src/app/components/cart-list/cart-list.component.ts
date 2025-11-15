import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CartItem } from '../../interfaces/cart-item';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart-list',
  imports: [
    CommonModule
  ],
  templateUrl: './cart-list.component.html',
  styleUrl: './cart-list.component.css'
})
export class CartListComponent {
 @Input() cartItems: CartItem[] = [];

 @Output() onDelete:any = new EventEmitter<any>();
 @Output() onQuantityUpdate: any = new EventEmitter<CartItem>();

 @Output() onUpdate:any = new EventEmitter<CartItem[]>()

  ngOnInit() {
  }

  increaseQuantity(item: CartItem) {
    item.quantity++;
    this.onQuantityUpdate.emit(item);
    this.onUpdate.emit(this.cartItems);
  }

  decreaseQuantity(item: CartItem) {
    if (item.quantity > 1) {
      item.quantity--;
      this.onQuantityUpdate.emit(item);
      this.onUpdate.emit(this.cartItems);
    }
  }

  onDeleteClick(cartId: string)
  {
    this.onDelete.emit(cartId);
  }
}
