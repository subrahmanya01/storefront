import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-manage-app',
  imports: [
    RouterModule
  ],
  templateUrl: './manage-app.component.html',
  styleUrl: './manage-app.component.css'
})
export class ManageAppComponent {
  constructor(private router: Router) {}
  
  isBannerLinkActive(): boolean {
    const url = this.router.url;
    console.log(url)
    return url === '/admin/manage' || url === '/admin/manage/banner';
  }
}
