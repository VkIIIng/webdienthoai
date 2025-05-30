using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SellPhoneMvcUI.Constants;
using SellPhoneMvcUI.Data;
using SellPhoneMvcUI.Models;
using System;
using System.Threading.Tasks;

namespace SellPhoneMvcUI.Services
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { Roles.Admin.ToString(), Roles.User.ToString() };


            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "daoduyhoangvuong@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Vương",
                    LastName = "Hoàng",
                    EmailConfirmed = true,
                    Address = "LA"
                };
                var result = await userManager.CreateAsync(adminUser, "Hoangvuong1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
                }
            }

        }
    }
}