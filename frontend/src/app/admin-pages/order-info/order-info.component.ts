import { Component, Input, OnInit } from '@angular/core';
import { OrderResponse, OrderStatus } from '../../interfaces/order';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { ProductInfoService } from '../../services/product-info.service';

@Component({
  selector: 'app-order-info',
  imports: [
    CommonModule,
    CurrencyPipe
  ],
  templateUrl: './order-info.component.html',
  styleUrl: './order-info.component.css'
})
export class OrderInfoComponent implements OnInit {
  @Input() order: OrderResponse | undefined;

  constructor(private activatedRoute: ActivatedRoute, private orderService: OrderService, private productInfoService: ProductInfoService){}

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.params["id"];
    if(id)
    {
      this.orderService.getOrder(id).subscribe({
        next: (data) => {
          this.order = data;
        },
        error: (err) => {
          console.error(err.message);
        }
      });
    }
  }
  
  getStatusText(status: OrderStatus): string {
    switch (status) {
      case OrderStatus.Pending:
        return 'Pending';
      case OrderStatus.Processing:
        return 'Processing';
      case OrderStatus.Completed:
        return 'Completed';
      case OrderStatus.Cancelled:
        return 'Cancelled';
      case OrderStatus.Refunded:
        return 'Refunded';
      default:
        return 'Unknown Status';
    }
  }
}
