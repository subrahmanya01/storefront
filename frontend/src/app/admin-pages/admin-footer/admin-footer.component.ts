import { Component, OnInit } from '@angular/core';
import { StoreInfoService } from '../../services/store-info.service';
import { StoreInfo } from '../../interfaces/storeinfo';

@Component({
  selector: 'app-admin-footer',
  imports: [],
  templateUrl: './admin-footer.component.html',
  styleUrl: './admin-footer.component.css'
})
export class AdminFooterComponent implements OnInit {
  year: number = new Date().getFullYear();
  storeInfo: StoreInfo = {} as StoreInfo;

  constructor(public readonly storeInfoService: StoreInfoService)
  {

  }

  ngOnInit(): void {
    this.storeInfoService.storeInfo$.subscribe({
      next: (data) => {
        this.storeInfo = data;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }
}
