using AutoMapper;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Entities.Enums;
using Storefront.OrderAndShippingService.Infrastructure.Repository;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;
using Storefront.OrderAndShippingService.Services.GrpcServices;

namespace Storefront.OrderAndShippingService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICartRpcService _cartRpcService;
        private readonly IOrderPriceService _orderPriceService;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, ICartRpcService cartRpcService, IOrderPriceService orderPriceService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _cartRpcService = cartRpcService;
            _orderPriceService = orderPriceService;
        }

        public async Task<OrderResponse?> CreateOrderAsync(Guid userId, CreateOrderRequest request)
        {
            var cartInfo = await _cartRpcService.GetCartByIdAsync(request.CartId);
            if (cartInfo == null) {
                return null;
            }

            var order = _mapper.Map<Order>(request);
            order.OrderNumber = HelperService.GenerateOrderNumber();

            order.Status = OrderStatus.Pending;
            order.CreatedAt = DateTime.UtcNow;
            order.CustomerId = userId;
            order.OrderDate = DateTime.UtcNow;

            var orderItems = _mapper.Map<List<OrderItem>>(cartInfo.Items);

            double totalAmount = 0;
            foreach (var item in orderItems) { 
                order.Items.Add(item);
                totalAmount += (double)item.Total;
            }
            var productInfo = new List<ProductInfomation>();
            foreach(var item in orderItems)
            {
                productInfo.Add(new ProductInfomation { ProductId = item.ProductId, VariantId = item.VariantId, Quantity = item.Quantity });
            }
            var orderPriceRequest = new OrderPriceRequest
            {
                CouponCode = request.CouponCode,
                PostalCode = request.ShippingAddress.PostalCode,
                SubTotal = totalAmount,
                ProductInfomations = productInfo
            };
            var orderAmount = await _orderPriceService.CalculateOrderPrice(orderPriceRequest);
            order.TotalAmount = (decimal)await _orderPriceService.GetTotalAmount(orderAmount);

            await _orderRepository.AddOrderAsync(order);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return order is not null ? _mapper.Map<OrderResponse>(order) : null;
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return _mapper.Map<List<OrderResponse>>(orders);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrders()
        {
            var result = await _orderRepository.GetAsync(x => true);
            return _mapper.Map<List<OrderResponse>>(result);
        }

        public async Task<OrderResponse?> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (existingOrder == null) {
                return null;
            }
            existingOrder.Status = request.OrderStatus;

            var result = await _orderRepository.UpdateOrderAsync(existingOrder);
            return _mapper.Map<OrderResponse>(result);
        }
        public async Task<bool> CancelOrder(Guid orderId)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null) {
                return false;
            }
            existingOrder.Status = OrderStatus.Cancelled;
            await _orderRepository.UpdateOrderAsync(existingOrder);
            return true;
        }

       
    }
}
