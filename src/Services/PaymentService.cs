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

        public Session CreateStripeCheckoutSession(Order order)
        {
            var lineItems = order.Items
                .Select(order => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmountDecimal = (decimal?)(order.UnitPrice * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = order.ProductName,
                        }
                    },
                    Quantity = order.Quantity,
                }).ToList();


            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = $"{_navigationManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_navigationManager.BaseUri}cart",
                LineItems = lineItems,
                Mode = "payment",
            };
            var service = new SessionService();
            var session = service.Create(options);

            return session;
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
