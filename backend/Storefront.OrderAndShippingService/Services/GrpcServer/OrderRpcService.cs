using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Storefront.OrderAndShippingService.Protos;

namespace Storefront.OrderAndShippingService.Services.GrpcServer
{
    public class OrderRpcService: OrderRpc.OrderRpcBase
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        private readonly IShippingChargeService _shippingChargeService;
        private readonly ILogger<OrderRpcService> _logger;
        public OrderRpcService(IDiscountService discountService, IMapper mapper, ILogger<OrderRpcService> logger, IShippingChargeService shippingChargeService)
        {
            _discountService = discountService;
            _mapper = mapper;
            _logger = logger;
            _shippingChargeService = shippingChargeService;
        }


        public override async Task<GetAllDiscountsResponse> GetAllDiscounts(Empty request, ServerCallContext context)
        {
            try
            {
                var result = await _discountService.GetAllDiscountsAsync();
                var response = _mapper.Map<List<DiscountRpcResponse>>(result);

                var grpcResponse = new GetAllDiscountsResponse();
                grpcResponse.Discounts.AddRange(response);

                return grpcResponse;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public override async Task<GetAllShippingChargesResponse> GetAllShippingCharges(Empty request, ServerCallContext context)
        {
            try
            {
                var result = await _shippingChargeService.GetAllAsync();
                var response = _mapper.Map<List<ShippingChargeRpcResponse>>(result);

                var grpcResponse = new GetAllShippingChargesResponse();
                grpcResponse.ShippingCharges.AddRange(response);

                return grpcResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
