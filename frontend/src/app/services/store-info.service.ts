import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { StoreInfo } from '../interfaces/storeinfo';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class StoreInfoService {

  private userServiceBase: string  = `${environment.gatewayUrl}/${environment.serviceBase.userServiceBase}`
  constructor(private readonly httpClient: HttpClient) { }

  public storeInfo = new BehaviorSubject<StoreInfo>({} as StoreInfo);
  storeInfo$ = this.storeInfo.asObservable();

  getStoreInfo():  Observable<any>
  {
    return this.httpClient.get<StoreInfo>(`${this.userServiceBase}/StoreInfo`);
  }
}
