using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SellPhoneMvcUI.Models;

namespace SellPhoneMvcUI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Electronics", Description = "Devices and gadgets" },
                new Category { CategoryId = 2, CategoryName = "Accessories", Description = "Phone accessories" },
                new Category { CategoryId = 3, CategoryName = "Wearables", Description = "Smartwatches and fitness trackers" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, CategoryId = 1, ProductName = "Smartphone X", Brand = "BrandA", Price = 699.99m, StockQuantity = 50, Description = "Latest smartphone with advanced features", ImageUrl = "https://images.samsung.com/is/image/samsung/p5/vn/smartphones/SM-A605FN_021_Front_Lavender.png?$ORIGIN_PNG$", CreatedAt = new DateTime(2025, 5, 25) },
                new Product { ProductId = 2, CategoryId = 1, ProductName = "Tablet Pro", Brand = "BrandB", Price = 499.99m, StockQuantity = 30, Description = "High-performance tablet", ImageUrl = "https://storage.micromagma.ma/micromagma/0cb7a714-d39d-4f30-958b-d652349dca33.png", CreatedAt = new DateTime(2025, 5, 26) },
                new Product { ProductId = 3, CategoryId = 2, ProductName = "Phone Case", Brand = "BrandC", Price = 19.99m, StockQuantity = 100, Description = "Durable phone case", ImageUrl = "https://hoanghamobile.com/Uploads/2023/12/14/a15-lte-3.png", CreatedAt = new DateTime(2025, 5, 27) },
                new Product { ProductId = 4, CategoryId = 3, ProductName = "Smartwatch Z", Brand = "BrandD", Price = 199.99m, StockQuantity = 20, Description = "Fitness tracking smartwatch", ImageUrl = "https://th.bing.com/th/id/OIP.qd_zK1LlhLDNXbGLCoR77wHaHa?rs=1&pid=ImgDetMain", CreatedAt = new DateTime(2025, 5, 26) }
            );
        }
    }
}
