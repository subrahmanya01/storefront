import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, HostListener, Input, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CarouselItem } from '../../interfaces/building-block/carousel-item';

@Component({
  selector: 'app-carousel',
  imports: [],
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.css'
})
export class CarouselComponent implements AfterViewInit {
  
  @ViewChild('scrollContainer', { static: false }) container:any;
  @Input() carouselItems: CarouselItem[] = [{
    imageUrl: "jbl-edv.png",
    productId: "54",
    navigationLink: ""
  },
  {
    imageUrl: "iphone-add.png",
    productId: "54",
    navigationLink: ""
  },
  {
    imageUrl: "jbl-edv.png",
    productId: "54",
    navigationLink: ""
  },
  {
    imageUrl: "iphone-add.png",
    productId: "54",
    navigationLink: ""
  }];

  containerViewWidth: number = 0;
  
  ngAfterViewInit(): void {
    this.updateWidth();
    this.autoScroll();
  }

  scrollToRight()
  {
    this.scrollContainer(this.containerViewWidth);
  }

  scrollToLeft()
  {
    this.scrollContainer(-this.containerViewWidth);
  }

  scrollContainer(amount: number) {
    if (this.container) {
      this.container.nativeElement.scrollTo({
        left: this.container.nativeElement.scrollLeft + amount,
        behavior: 'smooth'
      });

      setTimeout(() => {
      }, 300);
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateWidth();
  }

  autoScroll()
  {
    let count= 0;
    setInterval(()=>{
      if(count==this.carouselItems.length)
      {
        this.scrollContainer(-(this.containerViewWidth * this.carouselItems.length));
        count = 0;
      }
      else
      {
        this.scrollContainer(this.containerViewWidth);
        count++;
      }
    }, 4000);
  }

  updateWidth()
  {
    this.containerViewWidth = this.container.nativeElement.clientWidth;
  }
}
