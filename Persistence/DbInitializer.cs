using Domain.Contracts;
using Domain.Models.Identity;
using Domain.Models.Orders;
using Domain.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer(StoreDbContext _storeDbContext,
                 RoleManager<IdentityRole> _roleManager,
                 UserManager<ApplicationUser> _userManager) : IDbInitializer
    {
        public async Task InitializeAsync()
        {
            // In Deployment
            //if ((await _storeDbContext.Database.GetPendingMigrationsAsync()).Any())
            //    await _storeDbContext.Database.MigrateAsync();

            //In Development
            try
            {
                if (!_storeDbContext.Set<DeliveryMethod>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Persistence\Data\Seeding\deliveryMethods.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(data);

                    if (deliveryMethods is not null && deliveryMethods.Any())
                        _storeDbContext.Set<DeliveryMethod>().AddRange(deliveryMethods);

                    await _storeDbContext.SaveChangesAsync();
                }
                if (!_storeDbContext.Set<ProductBrand>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Persistence\Data\Seeding\brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(data);

                    if (brands is not null && brands.Any())
                        _storeDbContext.Set<ProductBrand>().AddRange(brands);

                    await _storeDbContext.SaveChangesAsync();
                }
                if (!_storeDbContext.Set<ProductType>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Persistence\Data\Seeding\types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(data);

                    if (types is not null && types.Any())
                        _storeDbContext.Set<ProductType>().AddRange(types);

                    await _storeDbContext.SaveChangesAsync();
                }
                if (!_storeDbContext.Set<Product>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Persistence\Data\Seeding\products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(data);

                    if (products is not null && products.Any())
                        _storeDbContext.Set<Product>().AddRange(products);

                    await _storeDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public async Task InitializeIdentityAsync()
        {
            // In Deployment
            //if ((await _storeDbContext.Database.GetPendingMigrationsAsync()).Any())
            //    await _storeDbContext.Database.MigrateAsync();

            //In Development
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!_userManager.Users.Any())
                {
                    var superAdmin = new ApplicationUser()
                    {
                        DisplayName = "Super Admin",
                        UserName = "superadmin",
                        Email = "superadmin@gmail.com"
                    };
                    var admin = new ApplicationUser()
                    {
                        DisplayName = "Admin",
                        UserName = "admin",
                        Email = "admin@gmail.com"
                    };
                    await _userManager.CreateAsync(superAdmin, "P@ssw0rd");
                    await _userManager.CreateAsync(admin, "password");

                    await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}
