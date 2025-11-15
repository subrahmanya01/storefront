import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { LS_CONTANTS } from '../constants';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const isLoggedIn = !!localStorage.getItem(LS_CONTANTS.ACCESS_TOKEN);

  if (!isLoggedIn) {
    router.navigate(['/login']);
    return false;
  }
  return true;
};
