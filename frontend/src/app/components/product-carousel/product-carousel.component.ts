import { AfterViewInit, Component, EventEmitter, HostListener, Input, Output, ViewChild } from '@angular/core';
import { ProductCardComponent } from '../product-card/product-card.component';
import { ExtendedProduct, ProductMeta } from '../../interfaces/product-meta';
import { NormalItems } from '../../interfaces/component-models';
import { NormalCardComponent } from '../normal-card/normal-card.component';
import { getProductsMeta } from '../../helpers/helper';

@Component({
  selector: 'app-product-carousel',
  imports: [
    ProductCardComponent,
    NormalCardComponent
  ],
  templateUrl: './product-carousel.component.html',
  styleUrl: './product-carousel.component.css'
})
export class ProductCarouselComponent implements AfterViewInit {
  @Input() title: string = "";
  @Input() cardWidth: number = 270; 
  @Input() cardType: "product" | "normal" = "product";
  @Input() normalItems: NormalItems[] = [];
  @Input() isDeleteWatchList: boolean = false;
  private _items: ExtendedProduct[] = [];
  @Input()
  set items(value: ExtendedProduct[]) {
    this._items = value;
    this.processItems(value);
  }
  
  get items(): ExtendedProduct[] {
    return this._items;
  }

  @ViewChild('cardView', { static: false }) container: any;
  containerWidth: number = 0;
  viewingWidth: number = 0;
  lastScroll: number = 0;
  oneScrollAmt: number = 0;
  isScrolling: boolean = false;

  productMeta: ProductMeta[] = [];

  ngAfterViewInit(): void {
    this.updateWidth();
  }

  scrollLeft() {
    this.scrollContainer(this.viewingWidth);
  }

  scrollRight() {
    this.scrollContainer(-this.viewingWidth);
  }

  processItems(value: ExtendedProduct[])
  {
    this.productMeta = getProductsMeta(value);
  }

  scrollContainer(amount: number) {
    if (this.container) {
      this.isScrolling = true; 
      this.container.nativeElement.scrollTo({
        left: this.container.nativeElement.scrollLeft + amount,
        behavior: 'smooth'
      });

      
      setTimeout(() => {
        this.isScrolling = false;
      }, 300); 
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateWidth();
  }

  updateWidth() {
    if (!this.container) return;
    let element = this.container.nativeElement;
    this.containerWidth = element.scrollWidth;
    this.viewingWidth = element.clientWidth;
    this.oneScrollAmt = this.viewingWidth;
  }
}
