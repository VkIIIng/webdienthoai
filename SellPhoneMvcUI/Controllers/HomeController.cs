using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SellPhoneMvcUI.Models.DTOs;

namespace SellPhoneMvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sTerm = "", int categoryId = 0)
        {
            IEnumerable<Product> products =await this._homeRepository.GetProductsAsync(sTerm, categoryId);
            IEnumerable<Category> categories = await this._homeRepository.GetCategoriesAsync();
            ProductDisplayModel model = new ProductDisplayModel
            {
                Products = products,
                Categories = categories,
                STerm=sTerm,
                CategoryId=categoryId,
            };
            return View(model);
        }

        public async Task<IActionResult> listProduct(string sTerm = "", int categoryId = 0)
        {
            IEnumerable<Product> products = await this._homeRepository.GetProductsAsync(sTerm, categoryId);
            IEnumerable<Category> categories = await this._homeRepository.GetCategoriesAsync();
            ProductDisplayModel model = new ProductDisplayModel
            {
                Products = products,
                Categories = categories,
                STerm = sTerm,
                CategoryId = categoryId,
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ProductDetail(int ProductId)
        {
            var product = await _homeRepository.GetProductById(ProductId);
            if (product == null)
                RedirectToAction("Index", "Home");
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult contact()
        {
            return View();
        }
    }
}
