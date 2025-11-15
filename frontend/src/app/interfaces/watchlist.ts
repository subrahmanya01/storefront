export interface WatchListResponse {
    whishListId: string;
    productId: string | null;
    createdAt: string,
    modifiedAt: string;
}

export interface AddWatchListRequest
{
    productId: string;
}

export interface RemoveWatchListItemRequest extends AddWatchListRequest
{

}