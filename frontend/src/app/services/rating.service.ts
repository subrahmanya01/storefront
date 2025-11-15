import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Rating, RatingRequest } from '../interfaces/rating';

@Injectable({
  providedIn: 'root'
})
export class RatingService {
  private apiBaseUrl = `${environment.gatewayUrl}/${environment.serviceBase.productServiceBase}`; 

  constructor(private http: HttpClient) { }

  getRating(productId: string)
  {
    return this.http.get<Rating[]>(`${this.apiBaseUrl}/api/ProductRating/${productId}`);
  }
  
  addRating(ratingRequest: RatingRequest)
  {
    return this.http.post<Rating>(`${this.apiBaseUrl}/api/ProductRating`,ratingRequest );
  }

  getUserRatings()
  {
    return this.http.get<Rating[]>(`${this.apiBaseUrl}/api/ProductRating/UserRatings`);
  }

  deleteRating(ratingId: string)
  {
    return this.http.delete<void>(`${this.apiBaseUrl}/api/ProductRating/${ratingId}`);
  }
}
