export interface ShippingChargeRequest {
    country: string | null;
    region: string | null;
    productId: string | null;
    minOrderAmount: number;
    maxOrderAmount: number | null;
    shippingFeePerKm: number;
    isFree: boolean;
    carrier: string | null;
    effectiveFrom: string | null;
    effectiveTo: string | null;
}

export interface ShippingChargeResponse {
    id: string;
    country: string | null;
    region: string | null;
    productId: string | null;
    minOrderAmount: number;
    maxOrderAmount: number | null;
    shippingFeePerKm: number;
    isFree: boolean;
    carrier: string | null;
    effectiveFrom: string | null;
    effectiveTo: string | null;
}