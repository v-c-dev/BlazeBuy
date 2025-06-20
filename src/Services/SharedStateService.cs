using BlazeBuy.Services.Interfaces;

namespace BlazeBuy.Services
{
    public sealed class SharedStateService
    {
        public event Action OnStateChange;
        private readonly IShoppingCartService _cartSvc;

        public SharedStateService(IShoppingCartService cartSvc)
        {
            _cartSvc = cartSvc;
        }

        public event Action<int>? CartCountChanged;

        private int _totalCartCount;
        public int TotalCartCount
        {
            get => _totalCartCount;
            private set
            {
                if (_totalCartCount == value) return;
                _totalCartCount = value;
                CartCountChanged?.Invoke(value);
            }
        }

        public async Task RefreshAsync(string userId, CancellationToken ct = default)
        {
            var count = await _cartSvc.GetTotalCartCountAsync(userId, ct);
            TotalCartCount = count;
        }

        public void Bump(int delta = 1)
        {
            Interlocked.Add(ref _totalCartCount, delta);
            CartCountChanged?.Invoke(_totalCartCount);
        }
        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
