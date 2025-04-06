using EShop.Domain.Repositories;
using EShopService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Seeders
{
    public  class EShopSeeder(DataContext context) :IESeeder
    {

        public async Task Seed()
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>
            {
                new Product { Name = "Shirt" },
                new Product { Name = "Jeans"},
                new Product { Name = "Bag" }
            };
                context.Products.AddRange(products);
                context.SaveChanges();
                var seededProducts = context.Products.ToList();
                foreach (var product in seededProducts)
                {
                    Console.WriteLine($"Seeded product: {product.Name}");
                }
            }
        }
    }
}
