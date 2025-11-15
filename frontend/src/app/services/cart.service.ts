import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { AddCartItemRequest, CartItemResponse, CartResponse, UpdateQuantityRequest } from '../interfaces/cart-item';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  public cartInfo = new BehaviorSubject<CartResponse|null>(null);
  cartInfo$ = this.cartInfo.asObservable();

  private cartServiceBase: string  = `${environment.gatewayUrl}/${environment.serviceBase.cartServiceBase}`
  constructor(private readonly httpClient: HttpClient) { }
  
  getCartItems() : Observable<CartResponse>
  {
    return this.httpClient.get<CartResponse>(`${this.cartServiceBase}/Api/Cart/get`);
  }

  addCartItem(model : AddCartItemRequest) : Observable<any>
  {
    return this.httpClient.post<any>(`${this.cartServiceBase}/Api/Cart/Add-Item`, model);
  }

  updateQuantity(itemId: string, model : UpdateQuantityRequest) : Observable<CartItemResponse>
  {
    return this.httpClient.put<CartItemResponse>(`${this.cartServiceBase}/Api/Cart/Update-Quantity/${itemId}`, model);
  }

  deleteCartItem(itemId: string) : Observable<CartResponse>
  {
    return this.httpClient.delete<CartResponse>(`${this.cartServiceBase}/Api/Cart/Remove-Item/${itemId}`);
  }

  clearCart() : Observable<CartResponse>
  {
    return this.httpClient.delete<CartResponse>(`${this.cartServiceBase}/Api/Cart/Clear`);
  }
}
