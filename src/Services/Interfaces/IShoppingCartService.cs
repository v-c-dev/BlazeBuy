﻿using BlazeBuy.Models;

namespace BlazeBuy.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddCartItemAsync(string userId, int productId, int qty = 1, CancellationToken ct = default);
        Task RemoveCartItemAsync(string userId, int productId, CancellationToken ct = default);
        Task<bool> ClearCartAsync(string userId, CancellationToken ct = default);
        Task<IReadOnlyList<ShoppingCart>> GetCartAsync(string userId, CancellationToken ct = default);
        Task<int> GetTotalCartCountAsync(string userId, CancellationToken ct = default);
        Task<bool> UpdateCartAsync(string userId, int productId, int deltaQty, CancellationToken ct = default);
    }
}
