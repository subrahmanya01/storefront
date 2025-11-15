import { CanDeactivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { LS_CONTANTS } from '../constants';

export const deactivateLoginGuard: CanDeactivateFn<unknown> = (component, currentRoute, currentState, nextState) => {
  const router = inject(Router);

  if (localStorage[LS_CONTANTS.ACCESS_TOKEN]) {
    router.navigateByUrl("/");
    return false;
  }
  
  return true;
};

