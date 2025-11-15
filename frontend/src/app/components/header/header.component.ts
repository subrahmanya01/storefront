import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LS_CONTANTS } from '../../constants';
import { AuthService } from '../../services/auth.service';
import { StoreInfoService } from '../../services/store-info.service';
import { StoreInfo } from '../../interfaces/storeinfo';
import { CartService } from '../../services/cart.service';
import { CartResponse } from '../../interfaces/cart-item';

@Component({
  selector: 'app-header',
  imports: [
    RouterModule
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {
  searchKey: string = "";
  storeInfo: StoreInfo = {} as StoreInfo;
  cartInfo: CartResponse | null  = {} as CartResponse;
  constructor(private readonly authService: AuthService, 
    private readonly router: Router, 
    private readonly cartService: CartService,
    private readonly storeInfoService: StoreInfoService,
    private readonly route: ActivatedRoute){}

  ngOnInit(): void {
    this.storeInfoService.storeInfo$.subscribe({
      next: (data:StoreInfo)=>{
        this.storeInfo = data;
      }
    })
    this.cartService.cartInfo$.subscribe({
      next: (data) => {
        this.cartInfo = data;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
    this.getCartInfo();
     this.route.queryParams.subscribe(params => {
          const keyword = params['key'];
          this.searchKey = keyword ?? "";
    });
      
  }
  canShowWithoutSignup():boolean
  {
    const token = localStorage.getItem(LS_CONTANTS.ACCESS_TOKEN);
    return !Boolean(token)
  }

  getCartInfo()
  {
    this.cartService.getCartItems().subscribe({
      next: (data) => {
        this.cartService.cartInfo.next(data);
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  logout()
  {
    this.authService.logout();
  }

  onSearch(keyword: string)
  {
    this.router.navigateByUrl(`/search?key=${keyword}`);
  }
}
