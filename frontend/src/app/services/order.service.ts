import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { CreateOrderRequest, OrderPriceRequest, OrderPriceResponse, OrderResponse, UpdateOrderStatusRequest } from '../interfaces/order';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private userServiceBase: string  = `${environment.gatewayUrl}/${environment.serviceBase.orderServiceBase}`
  constructor(private readonly httpClient: HttpClient) { }

  placeOrder(model: CreateOrderRequest) : Observable<any>
  {
    return this.httpClient.post<any>(`${this.userServiceBase}/Order/Create`, model);
  }
  
  getOrder(id:string) : Observable<OrderResponse>
  {
    return this.httpClient.get<OrderResponse>(`${this.userServiceBase}/Order/${id}`);
  }
  
  getOrderByCustomerId() : Observable<OrderResponse[]>
  {
    return this.httpClient.get<OrderResponse[]>(`${this.userServiceBase}/Order/Customer`);
  }

  getAllOrders() : Observable<OrderResponse[]>
  {
    return this.httpClient.get<OrderResponse[]>(`${this.userServiceBase}/Order/All`);
  }

  cancelOrder(orderId: string) : Observable<void>
  {
    return this.httpClient.delete<void>(`${this.userServiceBase}/Order/Cancel/${orderId}`);
  }

  updateOrderStatus(model: UpdateOrderStatusRequest) : Observable<OrderResponse>
  {
    return this.httpClient.put<OrderResponse>(`${this.userServiceBase}/Order/Status`, model);
  }

  getOrderPriceInfo(model: OrderPriceRequest) : Observable<OrderPriceResponse>
  {
    return this.httpClient.post<OrderPriceResponse>(`${this.userServiceBase}/OrderPrice/GetOrderAmount`, model);
  }
  
}
