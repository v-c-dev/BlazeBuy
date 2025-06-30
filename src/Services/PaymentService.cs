using BlazeBuy.Models;
using BlazeBuy.Models.Enums;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Stripe;
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

        private class LineAllocation
        {
            public OrderItem Item { get; set; } = default!;
            public long UnitCents { get; set; }
            public long LineTotal { get; set; }
            public long DiscountCents { get; set; }
        }

        public async Task<Session> CreateStripeCheckoutSessionAsync(Order order, CancellationToken ct = default)
        {
            var discountCents = (long)Math.Round((order.DiscountAmount ?? 0m) * 100m);

            var stripeItems = order.Items.Select(i => new SessionLineItemOptions
            {
                Quantity = i.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)Math.Round(i.UnitPrice * 100m),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = i.ProductName
                    }
                }
            }).ToList();

            var allocations = order.Items.Select(i => new LineAllocation
            {
                Item = i,
                UnitCents = (long)Math.Round(i.UnitPrice * 100m),
                LineTotal = (long)Math.Round(i.UnitPrice * 100m) * i.Quantity,
                DiscountCents = 0
            }).ToList();

            var cartTotalCents = allocations.Sum(a => a.LineTotal);
            foreach (var a in allocations)
                a.DiscountCents = (long)Math.Floor((decimal)a.LineTotal * discountCents / cartTotalCents);

            var allocatedSum = allocations.Sum(a => a.DiscountCents);
            var remainder = discountCents - allocatedSum;
            if (remainder > 0)
            {
                foreach (var a in allocations
                         .OrderByDescending(a => a.LineTotal)
                         .Take((int)remainder))
                {
                    a.DiscountCents++;
                }
            }

            stripeItems = allocations.Select(a =>
            {
                var netLine = a.LineTotal - a.DiscountCents;
                var finalPerUnit = netLine / a.Item.Quantity;
                return new SessionLineItemOptions
                {
                    Quantity = a.Item.Quantity,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = finalPerUnit,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = a.Item.ProductName
                        }
                    }
                };
            }).ToList();

            var promoCodeId = order.Coupon?.StripePromotionCodeId;

            var options = new SessionCreateOptions
            {
                Mode = "payment",
                // creates a new line for user to add coupons
                //AllowPromotionCodes = true,
                LineItems = stripeItems,

                Discounts = !string.IsNullOrEmpty(promoCodeId) ? new List<SessionDiscountOptions>
                    {
                        new SessionDiscountOptions
                        {
                            PromotionCode = promoCodeId
                        }
                    } : null,

                SuccessUrl = $"{_navigationManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_navigationManager.BaseUri}cart",
                CustomerEmail = order.Email,
                Metadata = new Dictionary<string, string>
                {
                    ["orderId"] = order.Id.ToString()
                }
            };

            return await new SessionService().CreateAsync(options, null, ct);
        }

        public async Task<Order> CheckOrderStatusAndUpdateOrder(string sessionId)
        {
            var svc = new SessionService();
            var session = await svc.GetAsync(sessionId);

            for (int i = 0; i < 4 && session.PaymentStatus != "paid"; i++)
            {
                await Task.Delay(400);
                session = await svc.GetAsync(sessionId);
            }

            var order = await _orderService.GetOrderBySessionIdAsync(sessionId);
            if (session.PaymentStatus.Equals("paid", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var item in order.Items)
                {
                    if (!await _productRepo.AdjustQuantityAsync(item.ProductId, -item.Quantity))
                        throw new InvalidOperationException($"Not enough stock for {item.ProductName}");
                }

                await _orderService.UpdateOrderStatusAsync(
                    order.Id,
                    OrderStatus.Approved,
                    session.PaymentIntentId);

                order.Status = OrderStatus.Approved;
            }

            return order;
        }
    }
}
