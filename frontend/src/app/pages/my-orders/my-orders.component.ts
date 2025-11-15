import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../services/order.service';
import { OrderItemResponse, OrderResponse, OrderStatus } from '../../interfaces/order';
import { SnackbarService } from '../../services/snackbar.service';
import { ProductInfoService } from '../../services/product-info.service';
import { ExtendedProduct, ProductsByIdsRequest } from '../../interfaces/product-meta';
import { RatingComponent } from '../../building-blocks/rating/rating.component';
import { RatingService } from '../../services/rating.service';

interface Order {
  id: string;
  orderId: string;
  date: string;
  status: 'Pending'| 'Processing'| 'Completed'| 'Cancelled'|'Refunded';
  items: any[];
  total: number;
  statusCode?: OrderStatus;
}

@Component({
  selector: 'app-my-orders',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.css'
})
export class MyOrdersComponent implements OnInit{
  orderInfo: OrderResponse[] = [];
  productInfo: ExtendedProduct[] = [];
  currentOrders: Order[] = [];
  previousOrders: Order[] = [];
  filterStatus: string = ''; 
  availableStatuses: string[] = ['All', 'Pending', 'Processing', 'Completed', 'Cancelled', 'Refunded'];

  filteredPreviousOrders: Order[] = [];

  constructor(private readonly orderService: OrderService,
    private readonly productInfoService: ProductInfoService,
    private readonly ratingService: RatingService,
    private readonly snackbar: SnackbarService
  ) { 
    
  }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders()
  {
    this.orderService.getOrderByCustomerId().subscribe({
      next:(data:OrderResponse[])=>{
        this.orderInfo = data;
        this.genarateOrderModal(data);
      },
      error:(err: any)=>{
        this.snackbar.error("Failed to load order details")
      }
    })
  }

  genarateOrderModal(orders: OrderResponse[])
  {
    const productIds: string[] = orders.flatMap(
      (order: OrderResponse) => order.orderItems.map(
        (item: OrderItemResponse) => item.productId
      )
    );
    if(!productIds)
    {
      return;
    }
    const request = {productIds: productIds} as ProductsByIdsRequest;
    this.productInfoService.getProductsByIds(request).subscribe({
      next: (data) => {
        this.productInfo = data;
        const result: Order[] = [];
        for(let order of orders)
        {
          result.push({
            id: order.orderNumber,
            orderId: order.id,
            date: new Date(order.createdAt),
            status: OrderStatus[order.status],
            statusCode: order.status,
            total: order.totalAmount,
            items: order.orderItems.map(x=> {
              const item = this.productInfo.find(y=> y.id == x.productId);
              const variant = item?.variants?.find(y=> y.id == x.variantId);
              return { product: item?.name, quantity: x.quantity, price: x.unitPrice, subtotal: x.quantity* x.unitPrice, imageUrl: variant?.images[0]?? 'https://placehold.co/50x50/E91E63/white?text=Product'}
            })
          } as any);
        }
        this.currentOrders = result.filter(x=> x.statusCode == OrderStatus.Pending || x.statusCode == OrderStatus.Processing ).reverse();
        this.filteredPreviousOrders = result.filter(x=> x.statusCode == OrderStatus.Cancelled || x.statusCode == OrderStatus.Refunded || x.statusCode == OrderStatus.Completed).reverse();
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  getStatusName(st: number)
  {
    return this.availableStatuses[st+1];
  }

  cancelOrder(id: string)
  {
    this.orderService.cancelOrder(id).subscribe({
      next: (data) => {
        this.snackbar.success("Order Cancelled successfully");
        this.getOrders();
      },
      error: (err) => {
        this.snackbar.error("Failed to cancel the order");
        console.error(err.message);
      }
    });
  }

  getStatusClass(status: number|undefined): string {
    switch (status) {
      case 0:
        return 'status-processing';
      case 1:
        return 'status-shipped';
      case 2:
        return 'status-delivered';
      case 3:
        return 'status-cancelled';
      case 4:
        return 'status-refunded';
      default:
        return '';
    }
  }
}
