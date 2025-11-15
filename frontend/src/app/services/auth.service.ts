
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { LS_CONTANTS } from '../constants';
import { Router } from '@angular/router';
import { UserService } from './user.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private accessToken: string | null = null;
  private refreshToken: string | null = null;

  private onCredUpdate = new BehaviorSubject<boolean>(false);
  onCredUpdate$ = this.onCredUpdate.asObservable();

  constructor(private readonly router: Router, private userService: UserService) {}

  setAccessToken(token: string) {
    this.accessToken = token;
    localStorage.setItem(LS_CONTANTS.ACCESS_TOKEN, this.accessToken);
    this.onCredUpdate.next(true);
  }

  setRefreshToken(token:string){
    this.refreshToken = token;
    localStorage.setItem(LS_CONTANTS.REFRESH_TOKEN, token);
    this.onCredUpdate.next(true);
  }

  getAccessToken(): string | null {
    return this.accessToken;
  }

  getRefreshToken(): string | null {
    return this.refreshToken;
  }

  resetUserInfo()
  {
    this.onCredUpdate.next(true);
  }
  isLoggedIn()
  {
    if(localStorage.getItem(LS_CONTANTS.ACCESS_TOKEN))
    {
      return true;
    }
    return false;
  }

  logout() {
    this.accessToken = null;
    localStorage.clear();
    this.router.navigateByUrl("/login");
  }
}
