export interface CreateOrderRequest {
    cartId: string;
    shippingAddress: ShippingAddress;
    couponCode: string | null;
}

export interface ShippingAddress {
    fullName: string;
    line1: string;
    line2: string | null;
    city: string;
    state: string;
    postalCode: string;
    country: string;
    phoneNumber: string | null;
    email: string | null;
}

export interface OrderResponse {
    id: string;
    customerId: string;
    orderNumber: string;
    totalAmount: number;
    orderItems: OrderItemResponse[];
    shippingAddress: ShippingAddress;
    billingAddress: BillingAddress | null;
    status: OrderStatus;
    createdAt: string;
}

export interface OrderItemResponse {
    productId: string;
    variantId: string;
    quantity: number;
    unitPrice: number;
}

export interface UpdateOrderStatusRequest {
    orderId: string;
    orderStatus: OrderStatus;
}

export interface BillingAddress extends ShippingAddress {

}

export enum OrderStatus {
    Pending,
    Processing,
    Completed,
    Cancelled,
    Refunded
}

export enum ShippingStatus {
    Preparing,
    Shipped,
    InTransit,
    Delivered,
    Returned
}

export interface OrderPriceRequest {
    productInfomations: ProductInfomation[];
    postalCode: string | null;
    couponCode: string | null;
    subTotal: number;
}

export interface ProductInfomation {
    productId: string | null;
    variantId: string | null;
    quantity: number;
}

export interface OrderPriceResponse {
    shippingFee: number | null;
    discounts: ProductDiscount[] | null;
    totalPrice: number | null;
    tax: number | null;
}

export interface ProductDiscount {
    productId: string | null;
    variantId: string | null;
    discount: number | null;
}