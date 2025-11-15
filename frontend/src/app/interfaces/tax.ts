export interface TaxRateRequest {
    country: string | null;
    state: string | null;
    rate: number;
    category: string | null;
}

export interface TaxRateResponse {
    id: string;
    country: string | null;
    state: string | null;
    rate: number;
    category: string | null;
}