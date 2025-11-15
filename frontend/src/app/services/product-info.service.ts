import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FlashSale, FlashSaleRequestModel } from '../interfaces/flashsales';
import { ExtendedProduct, ProductsByIdsRequest } from '../interfaces/product-meta';

@Injectable({
  providedIn: 'root'
})
export class ProductInfoService {
  private apiBaseUrl = `${environment.gatewayUrl}/${environment.serviceBase.productServiceBase}`; 

  constructor(private http: HttpClient) { }

  getProductsByIds(model: ProductsByIdsRequest): Observable<ExtendedProduct[]> {
    return this.http.post<ExtendedProduct[]>(`${this.apiBaseUrl}/ProductInfo/Products/Multiple/Get`, model)
  }

  getProductCategories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiBaseUrl}/ProductInfo/Categories`)
  }

  getProductBrands(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiBaseUrl}/ProductInfo/Brands`)
  }

  getProductByCategories(category: string): Observable<ExtendedProduct[]> {
    return this.http.get<ExtendedProduct[]>(`${this.apiBaseUrl}/ProductInfo/Categories/${category}`)
  }

  getFlashSaleProducts(): Observable<ExtendedProduct[]> {
    return this.http.get<ExtendedProduct[]>(`${this.apiBaseUrl}/ProductInfo/FlashSales/Products`)
  }

  getActiveFlashSaleList(): Observable<FlashSale[]> {
    return this.http.get<FlashSale[]>(`${this.apiBaseUrl}/ProductInfo/FlashSales`)
  }

  getAllFlashSaleList(): Observable<FlashSale[]> {
    return this.http.get<FlashSale[]>(`${this.apiBaseUrl}/ProductInfo/FlashSales/All`)
  }

  addFlashSales(request: FlashSaleRequestModel[]): Observable<FlashSale> {
    return this.http.post<FlashSale>(`${this.apiBaseUrl}/ProductInfo/FlashSales`, request)
  }

  deleteFlashSale(id:string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiBaseUrl}/ProductInfo/FlashSales/${id}`)
  }

  getBannerImages(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiBaseUrl}/ProductInfo/Banner`);
  }

  uploadBannerImages(formData: FormData): Observable<string[]> {
    return this.http.post<string[]>(`${this.apiBaseUrl}/ProductInfo/Banner`, formData);
  }

  removeBannerImage(imageUrl: string): Observable<void> {
    const params = new HttpParams().set('imageUrl', imageUrl);
    return this.http.delete<void>(`${this.apiBaseUrl}/ProductInfo/Banner`, { params });
  }
}
