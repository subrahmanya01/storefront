export interface FlashSaleRequestModel {
    productId: string | null;
    startsAt: string;
    endsAt: string;
}

export interface FlashSale
{
    id: string;
    productId: string | null;
    startsAt: string;
    endsAt: string;
    createdAt: string;
    modifiedAt: string;
}