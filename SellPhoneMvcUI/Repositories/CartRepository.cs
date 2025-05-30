using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SellPhoneMvcUI.Constants;

namespace SellPhoneMvcUI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItemAsync(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("user is not logged-in");
                }
                var cart = await GetCartAsync(userId);

                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };

                    _db.Carts.Add(cart);
                    _db.SaveChanges();
                }
                // cart item section
                var cartItem = _db.CartItems.FirstOrDefault(a => a.CartId == cart.CartId && a.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    cartItem = new CartItem
                    {
                        ProductId = productId,
                        CartId = cart.CartId,
                        Quantity = qty
                    };
                    _db.CartItems.Add(cartItem);
                }

                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            var cartItemCount = await GetCartItemCountAsync(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItemAsync(int productId, int qty = 1, bool removeAll = false)
        {
            string userId = GetUserId();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged-in");
                }

                var cart = await GetCartAsync(userId);
                if (cart is null)
                {
                    throw new Exception("Cart is empty");
                }

                // Find the cart item
                var cartItem = _db.CartItems.FirstOrDefault(a => a.CartId == cart.CartId && a.ProductId == productId);
                if (cartItem is null)
                {
                    throw new Exception("Item not found in cart");
                }

                // Handle removal logic
                if (removeAll)
                {
                    // Remove the entire item regardless of quantity
                    _db.CartItems.Remove(cartItem);
                }
                else
                {
                    if (qty <= 0)
                    {
                        throw new Exception("Quantity to remove must be greater than 0");
                    }

                    if (cartItem.Quantity <= qty)
                    {
                        // If qty to remove is greater than or equal to current quantity, remove the item
                        _db.CartItems.Remove(cartItem);
                    }
                    else
                    {
                        // Reduce the quantity by qty
                        cartItem.Quantity -= qty;
                    }
                }

                // Save changes to the database
                await _db.SaveChangesAsync();

                // Return the updated cart item count
                var cartItemCount = await GetCartItemCountAsync(userId);
                return cartItemCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to remove item: {ex.Message}");
            }
        }

        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Invalid User Id");

            var cart = await _db.Carts
                .Include(a => a.CartItems)
                .ThenInclude(a => a.Product)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(a => a.UserId == userId && !a.IsDeleted);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    CartItems = new List<CartItem>()
                };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            
            return cart;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            
            return userId;
        }

        public async Task<int> GetCartItemCountAsync(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
                userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
                return 0;

            var cart = await _db.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return 0;

            return cart.CartItems?.Sum(item => item.Quantity) ?? 0;
        }

        public async Task<bool> DoCheckout()
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var userId = GetUserId();
                if(string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged-in");
                var cart = await GetCartAsync(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                var cartItems = _db.CartItems.Include(c => c.Product)
                                    .Where(a => a.CartId == cart.CartId).ToList();
                if (cartItems.Count == 0)
                    throw new InvalidOperationException("Cart is empty");
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = Math.Round(cartItems.Sum(c => c.Quantity * c.Product.Price), 2),
                    StatusId = 1,
                    IsDeleted = false,
                    ShippingAddress = user.Address,
                };

                try
                {
                    _db.Orders.Add(order);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                }

                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.OrderId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price,
                    };
                    _db.OrderDetails.Add(orderDetail);
                }

                _db.SaveChanges();

                //Removing the cart details
                _db.CartItems.RemoveRange(cartItems);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
