import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject, delay, filter, map, Observable, tap } from 'rxjs';
export interface Breadcrumb {
  label: string;
  url: string;
}

@Injectable({
  providedIn: 'root'
})
export class BreadscrumService {
  private breadcrumbsSubject = new BehaviorSubject<Breadcrumb[]>([]);
  breadcrumbs$ = this.breadcrumbsSubject.asObservable();

  constructor(private router: Router) {
    
    const root = this.router.routerState.snapshot.root;
    const initialBreadcrumbs = this.buildBreadcrumbs(root);
    this.breadcrumbsSubject.next(initialBreadcrumbs);
  
    
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      tap(() => {
        const root = this.router.routerState.snapshot.root;
        const breadcrumbs = this.buildBreadcrumbs(root);
        this.breadcrumbsSubject.next(breadcrumbs);
      })
    ).subscribe();
  }

  private buildBreadcrumbs(route: ActivatedRouteSnapshot, url: string = '', breadcrumbs: Breadcrumb[] = []): Breadcrumb[] {
    if (route.routeConfig?.path) {
      const path = route.routeConfig.path;
      const segment = path
        .split('/')
        .map(part => part.startsWith(':') ? route.params[part.substring(1)] : part)
        .join('/');
      url += `/${segment}`;
    }

    if (route.data?.['breadcrumb']) {
      breadcrumbs.push({ label: route.data['breadcrumb'], url });
    }

    if (route.firstChild) {
      return this.buildBreadcrumbs(route.firstChild, url, breadcrumbs);
    }

    return breadcrumbs;
  }
}
