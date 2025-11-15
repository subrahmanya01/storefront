import { CanDeactivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { LS_CONTANTS } from '../constants';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
    sub: string;
    email: string;
    role?: string | string[];
    [key: string]: any;
  }

export const deactivateAdminGuard: CanDeactivateFn<unknown> = (component, currentRoute, currentState, nextState) => {
  const router = inject(Router);
  const token = localStorage[LS_CONTANTS.ACCESS_TOKEN];
  if (token) {
    const decoded = jwtDecode<JwtPayload>(token);
    console.log(decoded)
    if(decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]=="User")
    {
        router.navigateByUrl("");
        return false
    }
  }
  return true;
};

