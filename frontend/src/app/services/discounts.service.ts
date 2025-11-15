import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Discount, DiscountRequest, ValidateCouponRequest, ValidateCouponResponse } from '../interfaces/discounts';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DiscountsService {
  private userServiceBase: string  = `${environment.gatewayUrl}/${environment.serviceBase.orderServiceBase}`
  constructor(private readonly httpClient: HttpClient) { }

  createDiscountEntry(model: DiscountRequest) : Observable<Discount>
  {
    return this.httpClient.post<Discount>(`${this.userServiceBase}/Discounts/Create`, model);
  }

  updateDiscountEntry(id:string, model: DiscountRequest) : Observable<Discount>
  {
    return this.httpClient.put<Discount>(`${this.userServiceBase}/Discounts/Update/${id}`, model);
  }

  getAllDiscounts() : Observable<Discount[]>
  {
    return this.httpClient.get<Discount[]>(`${this.userServiceBase}/Discounts/All`);
  }

  getDiscountById(id:string) : Observable<Discount>
  {
    return this.httpClient.get<Discount>(`${this.userServiceBase}/Discounts/Get/${id}`);
  }

  searchDiscount(code:string| null = null, category: string| null = null, productId:string|null = null) : Observable<Discount[]>
  {
    let params = new HttpParams();

    if (code) {
      params = params.set('code', code);
    }
    if (category) {
      params = params.set('category', category);
    }
    if (productId) {
      params = params.set('productId', productId);
    }
  
    return this.httpClient.get<Discount[]>(`${this.userServiceBase}/Discounts/Search/`, { params });
  }

  validateCoupon(model: ValidateCouponRequest) : Observable<ValidateCouponResponse>
  {
    return this.httpClient.post<ValidateCouponResponse>(`${this.userServiceBase}/Discounts/Validate`, model);
  }

  deleteEntry(id:string) : Observable<boolean>
  {
    return this.httpClient.delete<boolean>(`${this.userServiceBase}/Discounts/Delete/${id}`);
  }
}
