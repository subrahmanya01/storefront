import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { UserResponse } from '../../interfaces/user';
import { AuthService } from '../../services/auth.service';
import { StoreInfo } from '../../interfaces/storeinfo';
import { StoreInfoService } from '../../services/store-info.service';

@Component({
  selector: 'app-admin-header',
  imports: [
    RouterModule
  ],
  templateUrl: './admin-header.component.html',
  styleUrl: './admin-header.component.css'
})
export class AdminHeaderComponent implements OnInit {
  storeInfo: StoreInfo = {} as StoreInfo;
  userName: string = "";

  constructor(private readonly userService: UserService, private readonly authService: AuthService, private readonly storeInfoService: StoreInfoService)
  {

  }

  ngOnInit(): void {
    this.storeInfoService.storeInfo$.subscribe({
      next: (data: StoreInfo) => {
        this.storeInfo = data;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
    this.userService.userInfo$.subscribe({
      next: (data: UserResponse|null)=>{
        this.userName = `${data?.firstName} ${data?.lastName}`;
      },
      error:(err: any)=>{
        console.error(err.message);
      }
    })
  }
  logout()
  {
    this.authService.logout();
  }

}
