import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { filter } from 'rxjs';
import { AdminHeaderComponent } from './admin-pages/admin-header/admin-header.component';
import { AdminFooterComponent } from './admin-pages/admin-footer/admin-footer.component';
import { AuthService } from './services/auth.service';
import { UserService } from './services/user.service';
import { UserResponse } from './interfaces/user';
import { SnackbarComponent } from './building-blocks/snackbar/snackbar.component';
import { SpinnerComponent } from './building-blocks/spinner/spinner.component';
import { VoiceRecognitionService } from './services/voice-recognition.service';
import { StoreInfoService } from './services/store-info.service';
import { StoreInfo } from './interfaces/storeinfo';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet, 
    HeaderComponent, 
    FooterComponent,
    AdminHeaderComponent,
    AdminFooterComponent,
    SnackbarComponent,
    SpinnerComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'storefront';
  trans: string = "";
  currentPath: string = "";
  isAdminRoute: boolean = false;
  isLoading: boolean = false;
  constructor(private readonly router: Router,
    private readonly authService: AuthService, 
    private readonly storeInfoService: StoreInfoService,
    private readonly userService: UserService)
  {
    
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.getStoreInfo();
    this.getAndSetUserInfo();
    this.router.events
    .pipe(filter(event => event instanceof NavigationEnd))
    .subscribe((event: NavigationEnd) => {
      this.currentPath = event.urlAfterRedirects;
      this.isAdminRoute = this.currentPath.includes("admin");
    });

    this.authService.onCredUpdate$.subscribe({
      next:(data: any)=>{
        this.getAndSetUserInfo();
      },
      error: (err: any)=>{
        console.error(err.message);
      }
    })
  }

  getAndSetUserInfo()
  {
    if(!this.authService.isLoggedIn())
    {
      return;
    }
    this.userService.getUser().subscribe({
      next:(data: UserResponse)=>{
        this.userService.userInfo.next(data);
      }
    });
  }

  getStoreInfo()
  {
    this.storeInfoService.getStoreInfo().subscribe({
      next: (data: StoreInfo) => {
        this.storeInfoService.storeInfo.next(data);
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }
}
