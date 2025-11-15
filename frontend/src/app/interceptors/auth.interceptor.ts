import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LS_CONTANTS } from '../constants';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router); 

  let authReq = req;
  const token = localStorage.getItem(LS_CONTANTS.ACCESS_TOKEN);

  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(
    catchError(error => {
      if (error.status === 401) {
        
        localStorage.removeItem(LS_CONTANTS.ACCESS_TOKEN);
        router.navigate(['/login']);
      }
      return throwError(() => error);
    })
  );
};
