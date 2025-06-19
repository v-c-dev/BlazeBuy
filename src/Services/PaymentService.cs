using System.Globalization;
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

        public PaymentService(NavigationManager navigationManager, IOrderService orderService)
        {
            _navigationManager = navigationManager;
            _orderService = orderService;
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
                    UnitAmount = (long)(i.UnitPrice * 100),   // cents ✔
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

            return await new SessionService().CreateAsync(options, null, ct);   // async ✔
        }

        public async Task<Order> CheckOrderStatusAndUpdateOrder(string sessionId)
        {
            Order order = await _orderService.GetOrderBySessionIdAsync(sessionId);
            var service = new SessionService();
            var session = service.Get(sessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                await _orderService.UpdateOrderStatusAsync(order.Id,
                    OrderStatus.Approved,
                    session.PaymentIntentId);
            }

            return order;
        }
    }
}
