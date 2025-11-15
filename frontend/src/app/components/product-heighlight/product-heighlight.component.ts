import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-product-heighlight',
  imports: [],
  templateUrl: './product-heighlight.component.html',
  styleUrl: './product-heighlight.component.css'
})
export class ProductHeighlightComponent {
 @Input() isFreeDel: boolean = false;
}
