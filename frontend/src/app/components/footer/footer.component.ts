import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { StoreInfoService } from '../../services/store-info.service';
import { StoreInfo } from '../../interfaces/storeinfo';

@Component({
  selector: 'app-footer',
  imports: [
    RouterModule
  ],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent implements OnInit {
  storeInfo: StoreInfo = {} as StoreInfo;
  year: number = new Date().getFullYear();
  constructor(private readonly storeInfoService: StoreInfoService){

  }
  ngOnInit(): void {
    this.storeInfoService.storeInfo$.subscribe({
      next: (data:StoreInfo)=>{
        this.storeInfo = data;
      }
    })
  }
}
