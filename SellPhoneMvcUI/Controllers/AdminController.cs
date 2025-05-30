using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellPhoneMvcUI.Constants;
using SellPhoneMvcUI.Models.DTOs;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace SellPhoneMvcUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // User Management
        [HttpGet("users")]
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        [HttpGet("users/edit/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Get all available roles
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            // Get the user's current roles
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                AllRoles = allRoles,
                SelectedRole = userRoles.FirstOrDefault() // Assuming a user has one role; adjust if multiple roles are possible
            };

            return View(model);
        }

        [HttpPost("users/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate AllRoles in case of validation failure
                model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user details
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Address = model.Address;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(model);
            }

            // Update user role
            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                // Check if the selected role exists
                if (!await _roleManager.RoleExistsAsync(model.SelectedRole))
                {
                    ModelState.AddModelError(string.Empty, "Selected role does not exist.");
                    model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                    return View(model);
                }

                // Get current roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                // Remove user from current roles
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Failed to remove current roles.");
                    model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                    return View(model);
                }

                // Add user to the new role
                var addResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Failed to assign new role.");
                    model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                    return View(model);
                }
            }

            return RedirectToAction("UserList");
        }

        [HttpPost("users/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction("UserList");
        }

        // Product Management
        [HttpGet("products")]
        public IActionResult ProductList()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        [HttpGet("products/create")]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost("products/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(model);
            }
            if (model == null)
                throw new Exception();
            Product p = new Product
            {
                CategoryId = model.CategoryId,
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                Brand = model.Brand,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
            };
            _context.Products.Add(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }
        
        [HttpGet("products/edit/{id}")]
        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        [HttpPost("products/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(model);
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            product.ProductName = model.ProductName;
            product.Brand = model.Brand;
            product.Price = model.Price;
            product.StockQuantity = model.StockQuantity;
            product.Description = model.Description;
            product.ImageUrl = model.ImageUrl;
            product.CategoryId = model.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }
        
        [HttpPost("products/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }

        [HttpGet("categorys")]
        public IActionResult CategoryList()
        {
            var products = _context.Categories.ToList();
            return View(products);
        }

        [HttpGet("categorys/create")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost("categorys/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (model == null)
                throw new Exception();
            Category p = new Category
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                Description = model.Description,
            };
            _context.Categories.Add(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("CategoryList");
        }

        [HttpGet("categorys/edit/{id}")]
        public IActionResult Editcategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost("categorys/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editcategory(int id, CategoryViewModel model)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            category.CategoryId = model.CategoryId;
            category.CategoryName = model.CategoryName;
            category.Description = model.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("CategoryList");
        }

        [HttpPost("categorys/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("CategoryList");
        }
    }
}