namespace Storefront.OrderAndShippingService.Entities.Enums
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled,
        Refunded
    }

    public enum ShippingStatus
    {
        Preparing,
        Shipped,
        InTransit,
        Delivered,
        Returned
    }

}
