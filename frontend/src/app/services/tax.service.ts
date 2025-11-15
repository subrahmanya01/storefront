import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaxRateRequest, TaxRateResponse } from '../interfaces/tax';

@Injectable({
  providedIn: 'root'
})
export class TaxService {
  private readonly baseUrl: string =`${environment.gatewayUrl}/${environment.serviceBase.orderServiceBase}/TaxRate`; 
  constructor(private readonly httpClient: HttpClient) { }

  create(model: TaxRateRequest): Observable<TaxRateResponse> {
    return this.httpClient.post<TaxRateResponse>(`${this.baseUrl}/Create`, model);
  }

  getById(id: string): Observable<TaxRateResponse> {
    return this.httpClient.get<TaxRateResponse>(`${this.baseUrl}/Get/${id}`);
  }

  getAll(): Observable<TaxRateResponse[]> {
    return this.httpClient.get<TaxRateResponse[]>(`${this.baseUrl}/All`);
  }

  update(id: string, model: TaxRateRequest): Observable<TaxRateResponse> {
    return this.httpClient.put<TaxRateResponse>(`${this.baseUrl}/Update/${id}`, model);
  }

  delete(id: string): Observable<boolean> {
    return this.httpClient.delete<boolean>(`${this.baseUrl}/Delete/${id}`);
  }
}

