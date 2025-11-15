import { ExtendedProduct } from "./product-meta";

export interface CartItem {
    cartId: string;
    name: string;
    price: number;
    quantity: number;
    imageUrl: string;
    variantId: string;
    productId: string;
    productData?: ExtendedProduct
}

export interface CartResponse
{
    id: string;
    userId: string;
    items: CartItemResponse[]
}

export interface CartItemResponse {
    id: string;
    productId: string;
    variantId: string;
    quantity: number;
    price: number;
    createdAt: string;
    modifiedAt: string;
}

export interface AddCartItemRequest {
    productId: string;
    productVariantId: string;
    quantity: number;
}

export interface UpdateQuantityRequest {
    quantity: number;
}