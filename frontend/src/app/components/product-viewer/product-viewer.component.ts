import { Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';

@Component({
  selector: 'app-product-viewer',
  imports: [],
  templateUrl: './product-viewer.component.html',
  styleUrl: './product-viewer.component.css'
})
export class ProductViewerComponent implements OnInit, OnChanges {
  @Input() images: string[] = [];
  sidebarImages: string[] = [];
  selectedImage: string = "";
  @ViewChild('mainImage') mainImage: any;

  constructor(private el: ElementRef) { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.images && this.images.length > 0) {
      this.selectedImage = this.images[0];
      this.sidebarImages = this.images.slice(1);
      const rightPanelImage = this.el.nativeElement.querySelector('.right-panel img');
      if(rightPanelImage) {
        rightPanelImage.classList.remove('loaded');
      }
    }
  }

  selectImage(image: string) {
    this.selectedImage = image;
    this.sidebarImages = [...this.images];

    const index = this.sidebarImages.indexOf(image);
    if (index !== -1) {
      this.sidebarImages.splice(index, 1);
    }
    const rightPanelImage = this.el.nativeElement.querySelector('.right-panel img');
      if(rightPanelImage) {
        rightPanelImage.classList.remove('loaded');
      }
  }

  imageLoaded(event: any) {
    event.target.classList.add('loaded');
  }
}
