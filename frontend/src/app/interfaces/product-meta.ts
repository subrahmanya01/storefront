import { Discount } from "./discounts";
import { ShippingChargeResponse } from "./shipping";

export interface ProductMeta {
    id:string;
    name: string;
    image: string;
    actualPrice: number;
    discountedPrice: number;
    rating: number;
    isLiked: boolean;
    variantId: string;
    data: ExtendedProduct;
    discount: number
}

export interface ProductVariant {
  id?: string; 
  attributes: { [key: string]: string };
  price: number;
  inventory: number;
  images: string[]; 
  isComplete?: boolean; 
}
  
export interface Product {
  id: string; 
  name: string;
  type: string;
  category: string;
  description: string;
  allowedAttributes: string[];
  baseAttributes: { [key: string]: string };
  variants?: ProductVariant[]; 
  attributeValues: { [key: string]: Set<string> };
}

export interface ExtendedProduct extends Product
{
  rating: number;
  discounts: Discount[],
  shippingCharges : ShippingChargeResponse[]
}

export interface ProductsByIdsRequest
{
  productIds: string[];
}