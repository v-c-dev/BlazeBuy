using BlazeBuy.Models;
using BlazeBuy.Models.Enums;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Stripe.Checkout;

namespace BlazeBuy.Services
{
    public class PaymentService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IOrderService _orderService;
        private readonly IProductRepository _productRepo;

        public PaymentService(
            NavigationManager navigationManager,
            IOrderService orderService,
            IProductRepository productRepo)
        {
            _navigationManager = navigationManager;
            _orderService = orderService;
            _productRepo = productRepo;
        }

        public async Task<Session> CreateStripeCheckoutSessionAsync(
            Order order, CancellationToken ct = default)
        {
            var items = order.Items.Select(i => new SessionLineItemOptions
            {
                Quantity = i.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(i.UnitPrice * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = i.ProductName
                    }
                }
            }).ToList();

            var options = new SessionCreateOptions
            {
                Mode = "payment",
                LineItems = items,
                SuccessUrl = $"{_navigationManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_navigationManager.BaseUri}cart",
                CustomerEmail = order.Email,
                Metadata = new() { ["orderId"] = order.Id.ToString() }
            };

            return await new SessionService().CreateAsync(options, null, ct);
        }

        public async Task<Order> CheckOrderStatusAndUpdateOrder(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            for (int i = 0; i < 4 && session.PaymentStatus != "paid"; i++)
            {
                await Task.Delay(400);
                session = await service.GetAsync(sessionId);
            }

            Order order = await _orderService.GetOrderBySessionIdAsync(sessionId);

            if (session.PaymentStatus.Equals("paid", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var item in order.Items)
                {
                    bool ok = await _productRepo.AdjustQuantityAsync(
                                  item.ProductId, -item.Quantity);
                    if (!ok)
                        throw new InvalidOperationException(
                            $"Not enough stock for {item.ProductName}");
                }

                await _orderService.UpdateOrderStatusAsync(
                          order.Id, OrderStatus.Approved, session.PaymentIntentId);
                order.Status = OrderStatus.Approved;
            }

            return order;
        }
    }

}
