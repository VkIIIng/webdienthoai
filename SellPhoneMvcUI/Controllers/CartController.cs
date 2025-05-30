using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SellPhoneMvcUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        public CartController(ICartRepository cartRepository) {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddItemAsync(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int productId, int qty=1, bool removeAll=false)
        {
            var cartCount = await _cartRepository.RemoveItemAsync(productId, qty, removeAll);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int total = await _cartRepository.GetCartItemCountAsync();
            return Ok(total);
        }

        public async Task<IActionResult> Checkout()
        {
            bool isCheckOut = await _cartRepository.DoCheckout();
            if (!isCheckOut)
                throw new Exception("Somthing happen in server side");
            return RedirectToAction("Index", "Home");
        }
    }
}
