import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-quantity-selector',
  imports: [],
  templateUrl: './quantity-selector.component.html',
  styleUrl: './quantity-selector.component.css'
})
export class QuantitySelectorComponent {
  @Input() quantity = 1;
  @Output() quantityChange = new EventEmitter<number>();

  decrement() {
    if (this.quantity > 1) {
      this.quantity--;
      this.quantityChange.emit(this.quantity);
    }
  }

  increment() {
    this.quantity++;
    this.quantityChange.emit(this.quantity);
  }

  onInputChange(event: any) {
    const value = parseInt(event.target.value, 10);
    if (!isNaN(value) && value >= 1) {
      this.quantity = value;
      this.quantityChange.emit(this.quantity);
    } else {
      
      this.quantity = 1;
      this.quantityChange.emit(1);
    }
  }
}
