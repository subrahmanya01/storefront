export interface DiscountRequest {
    code: string | null;
    percentage: number;
    minOrderAmount: number | null;
    category: string | null;
    productId: string | null;
    validFrom: string;
    validTo: string;
}

export interface Discount {
    id: string;
    code: string | null;
    percentage: number;
    minOrderAmount: number | null;
    category: string | null;
    productId: string | null;
    validFrom: string;
    validTo: string;
    createdAt: string;
    modifiedAt: string;
}

export interface ValidateCouponRequest {
    code: string;
    orderAmount: number;
    productId: string | null;
    category: string | null;
}

export interface ValidateCouponResponse {
    isValid: boolean;
    discountPercentage: number;
    message: string;
}