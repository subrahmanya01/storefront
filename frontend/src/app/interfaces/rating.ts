export interface Rating {
    id: string;
    productId: string;
    userId: string;
    stars: number;
    comment: string | null;
    timestamp: string;
}

export interface RatingRequest {
    productId: string;
    rating: number;
    comment: string | null;
}