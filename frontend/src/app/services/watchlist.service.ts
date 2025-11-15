import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { AddWatchListRequest, RemoveWatchListItemRequest, WatchListResponse } from '../interfaces/watchlist';

@Injectable({
  providedIn: 'root'
})
export class WatchlistService {
  private userServiceBase: string  = `${environment.gatewayUrl}/${environment.serviceBase.userServiceBase}`
  constructor(private readonly httpClient: HttpClient) { }

  addWatchList(model: AddWatchListRequest) : Observable<WatchListResponse[]>
  {
    return this.httpClient.post<WatchListResponse[]>(`${this.userServiceBase}/WhishList/Add`, model);
  }

  getWatchListItems() : Observable<WatchListResponse[]>
  {
    return this.httpClient.get<WatchListResponse[]>(`${this.userServiceBase}/WhishList`);
  }

  removeWatchListItem(model: RemoveWatchListItemRequest) : Observable<void>
  {
    return this.httpClient.post<void>(`${this.userServiceBase}/WhishList/remove`, model);
  }

  clearWatchList() : Observable<void>
  {
    return this.httpClient.delete<void>(`${this.userServiceBase}/WhishList/clear`);
  }
}
