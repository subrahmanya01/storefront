import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../services/order.service';
import { OrderResponse, UpdateOrderStatusRequest } from '../../interfaces/order';
import { SnackbarService } from '../../services/snackbar.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manage-orders',
  imports: [
    FormsModule,
    CommonModule
  ],
  templateUrl: './manage-orders.component.html',
  styleUrl: './manage-orders.component.css'
})
export class ManageOrdersComponent {

  allOrders: OrderResponse[] = []

  filterStatus: string = '';
  availableStatuses: string[] = ['All', 'Pending', 'Processing', 'Completed', 'Cancelled', 'Refunded'];

  sortBy: string = 'date'; 
  sortDirection: 'asc' | 'desc' = 'desc';

  displayedOrders: OrderResponse[] = [];
  constructor(private orderService: OrderService, private snackbar: SnackbarService, private readonly router: Router) { }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders()
  {
    this.orderService.getAllOrders().subscribe({
      next: (data) => {
        this.allOrders = data;
        this.applyFiltersAndSort();
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

  applyFiltersAndSort(): void {
    let filteredOrders = this.allOrders;

    
    if (this.filterStatus !== '' && this.filterStatus !== 'All') {
      filteredOrders = filteredOrders.filter(order => order.status === this.availableStatuses.indexOf(this.filterStatus)-1);
    }

    
    this.sortOrders(filteredOrders);
  }

  sortOrders(orders: OrderResponse[]): void {
    this.displayedOrders = orders.sort((a, b) => {
      const aValue = a[this.sortBy as keyof OrderResponse];
      const bValue = b[this.sortBy as keyof OrderResponse];

      if (aValue == null || bValue == null) return 0;

      if (aValue < bValue) {
        return this.sortDirection === 'asc' ? -1 : 1;
      }
      if (aValue > bValue) {
        return this.sortDirection === 'asc' ? 1 : -1;
      }
      return 0; 
    });
  }

  changeSort(column: string): void {
    if (this.sortBy === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = column;
      this.sortDirection = 'asc'; 
    }
    this.applyFiltersAndSort(); 
  }

  getStatusClass(status: number): string {
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

  viewOrderDetails(orderId: string): void {
    this.router.navigateByUrl(`/admin/manage/order/${orderId}`)
  }

  updateOrderStatus(order: OrderResponse, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const newStatus = selectElement.value as 'Pending'| 'Processing'| 'Completed'| 'Cancelled'|'Refunded';

    console.log(`Admin: Attempting to update status for order ${order.id} to ${newStatus}`);

    const orderToUpdate = this.allOrders.find(o => o.id === order.id);
    if (orderToUpdate) {
      orderToUpdate.status = this.availableStatuses.indexOf(newStatus)-1;

      const request = {orderId: order.id, orderStatus: orderToUpdate.status } as UpdateOrderStatusRequest;
      this.orderService.updateOrderStatus(request).subscribe({
        next: (data) => {
           this.snackbar.success("Status updated sucessfully");
        },
        error: (err) => {
          this.snackbar.error("Status update failed");
          console.error(err.message);
        }
      });
     
      console.log(`Status updated locally for order ${order.id}`);
      this.applyFiltersAndSort(); 
    } else {
      console.error(`Order with ID ${order.id} not found.`);
    }
  }
}
