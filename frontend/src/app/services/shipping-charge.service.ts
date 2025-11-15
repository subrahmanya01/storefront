import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { ShippingChargeRequest, ShippingChargeResponse } from '../interfaces/shipping';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShippingChargeService {

  private userServiceBase: string  = `${environment.gatewayUrl}/${environment.serviceBase.orderServiceBase}`
  constructor(private readonly httpClient: HttpClient) { }

  createShippingChargeEntry(model: ShippingChargeRequest) : Observable<ShippingChargeResponse>
  {
    return this.httpClient.post<ShippingChargeResponse>(`${this.userServiceBase}/ShippingCharge/Create`, model);
  }

  updateShippingChargeEntry(id: string, model: ShippingChargeRequest) : Observable<ShippingChargeResponse>
  {
    return this.httpClient.put<ShippingChargeResponse>(`${this.userServiceBase}/ShippingCharge/Update/${id}`, model);
  }

  getShippingCharge(id:string) : Observable<ShippingChargeResponse>
  {
    return this.httpClient.get<ShippingChargeResponse>(`${this.userServiceBase}/ShippingCharge/Get/${id}`);
  }

  getAll(id:string) : Observable<ShippingChargeResponse[]>
  {
    return this.httpClient.get<ShippingChargeResponse[]>(`${this.userServiceBase}/ShippingCharge/All`);
  }

  deleteEntry(id:string) : Observable<void>
  {
    return this.httpClient.delete<void>(`${this.userServiceBase}/ShippingCharge/Delete/${id}`);
  }

  getPostalCodes(): Observable<string[]>
  {
    return this.httpClient.get<string[]>(`${this.userServiceBase}/ShippingCharge/GetPostalCodes`);
  }
}
