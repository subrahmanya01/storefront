export interface SearchRequest {
    keyword: string | null;
    filters: SearchFilters | null;
}

export interface SearchFilters {
    category: string | null;
    brand: string | null;
    priceStart: number | null;
    priceEnd: number | null;
}