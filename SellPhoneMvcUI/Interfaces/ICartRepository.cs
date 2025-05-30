namespace SellPhoneMvcUI.Interfaces
{
    public interface ICartRepository
    {
        Task<int> AddItemAsync(int productId, int qty=1);
        Task<int> RemoveItemAsync(int productId, int qty=1, bool removeAll = false);
        Task<Cart> GetUserCart();
        Task<int> GetCartItemCountAsync(string userId = "");
        Task<bool> DoCheckout();
    }
}
