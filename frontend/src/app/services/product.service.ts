import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Product, ProductVariant } from '../interfaces/product-meta';
import { environment } from '../../environments/environment.development';
import { SearchRequest } from '../interfaces/search';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiBaseUrl = `${environment.gatewayUrl}/${environment.serviceBase.productServiceBase}`; 

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiBaseUrl}/product`)
  }

  getProduct(id: string): Observable<Product> {
   return this.http.get<Product>(`${this.apiBaseUrl}/product/${id}`)
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(`${this.apiBaseUrl}/product`, product);
  }

  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(`${this.apiBaseUrl}/product`, product);
  }

  deleteProduct(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/product/${id}`)
  }

  addVariantToProduct(productId: string, variant: ProductVariant): Observable<ProductVariant> {
    return this.http.post<ProductVariant>(`${this.apiBaseUrl}/products/${productId}/variants`, variant)
  }

  updateVariantForProduct(productId: string, variant: ProductVariant): Observable<ProductVariant> {
    return this.http.put<ProductVariant>(`${this.apiBaseUrl}/products/${productId}/variants/${variant.id}`, variant)
  }

  deleteVariantFromProduct(productId: string, variantId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/products/${productId}/variants/${variantId}`)
  }

  uploadImagesForVariant(productId: string, variantId: string, formData: FormData): Observable<string[]> {
    return this.http.post<string[]>(`${this.apiBaseUrl}/products/${productId}/variants/${variantId}/images/multiple`, formData);
  }
  removeImageFromVariant(productId: string, variantId: string, imageUrls: string[]): Observable<void> {
    if (!imageUrls || imageUrls.length === 0) {
      console.warn('No image URLs provided for deletion.');
      return of(undefined); 
    }

    let params = new HttpParams();
    imageUrls.forEach(url => {
      params = params.append('imageUrls', url);
    });

    return this.http.delete<void>(`${this.apiBaseUrl}/products/${productId}/variants/${variantId}/Images`, { params: params });
  }

  searchResults(request: SearchRequest): Observable<Product[]>{
   return this.http.post<Product[]>(`${this.apiBaseUrl}/ProductSearch/SearchProduct`, request);
  }
}