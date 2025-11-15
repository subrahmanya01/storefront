import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedDataAccessService {
  public onWatchlistRemove = new BehaviorSubject<string>("");
  onWatchlistRemove$ = this.onWatchlistRemove.asObservable();

  constructor() { }

  setWatchListRemoveItem(productId:string)
  {
    this.onWatchlistRemove.next(productId);
  }
}
