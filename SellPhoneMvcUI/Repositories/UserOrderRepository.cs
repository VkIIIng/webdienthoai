using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SellPhoneMvcUI.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not logged-in");
            var userOrders =await _db.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .ThenInclude(o => o.Category)
                .Where(o => o.UserId == userId).ToListAsync();
            return userOrders;
        }
    }
}
