using Eshop.Application;
using EShop.Domain.Repositories;
using EShopService.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace EShopService.IntegrationTests.Controllers
{
    public class ProductControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public ProductControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
      
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

          
                        if (dbContextOptions != null)
                            services.Remove(dbContextOptions);

                        services.AddDbContext<DataContext>(options =>
                            options.UseInMemoryDatabase("ProductsDbForTest"));
                    });
                });

            _client = _factory.CreateClient();
        }



        [Fact]
        public async Task GetProductByID_ExistingProduct_ReturnsOkWithProduct()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                dbContext.Products.RemoveRange(dbContext.Products);
                await dbContext.SaveChangesAsync();

                var product = new Product
                {
                    Id = 1,
                    Name = "testowy",

                };
                dbContext.Products.Add(product);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/Product/getProductByID?productID=1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var returnedProduct = await response.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(returnedProduct);
            Assert.Equal(1, returnedProduct.Id);
            Assert.Equal("testowy", returnedProduct.Name);

        }

        [Fact]
        public async Task GetProductByID_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                dbContext.Products.RemoveRange(dbContext.Products);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/Product/getProductByID?productID=999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal("product not found", responseContent.Error);
        }



        [Fact]
        public async Task DeleteProductByID_ExistingProduct_ReturnsOk()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                dbContext.Products.RemoveRange(dbContext.Products);
                await dbContext.SaveChangesAsync();

         
                var product = new Product
                {
                    Id = 1,
                    Name = "testowy"
                };
                dbContext.Products.Add(product);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/Product/deleteProductByID?productID=1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var product = await dbContext.Products.FindAsync(1);
                Assert.Null(product);
            }
        }

        [Fact]
        public async Task DeleteProductByID_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

       
                dbContext.Products.RemoveRange(dbContext.Products);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/Product/deleteProductByID?productID=999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal("product not found", responseContent.Error);
        }



        [Fact]
        public async Task GetAllProducts_ProductsExist_ReturnsOkWithProducts()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        
                dbContext.Products.RemoveRange(dbContext.Products);
                await dbContext.SaveChangesAsync();

                dbContext.Products.AddRange(
                    new Product { Id = 1, Name = "myszka" },
                    new Product { Id = 2, Name = "winogrona" },
                    new Product { Id = 3, Name = "kawa" }
                );
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/Product/getAllProducts");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var returnedProducts = await response.Content.ReadFromJsonAsync<List<Product>>();
            Assert.NotNull(returnedProducts);
            Assert.Equal(3, returnedProducts.Count);

            Assert.Contains(returnedProducts, p => p.Id == 1 && p.Name == "myszka");
            Assert.Contains(returnedProducts, p => p.Id == 2 && p.Name == "winogrona");
            Assert.Contains(returnedProducts, p => p.Id == 3 && p.Name == "kawa");
        }

        [Fact]
        public async Task GetAllProducts_NoProductsExist_ReturnsOkWithMessage()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();


                dbContext.Products.RemoveRange(dbContext.Products);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/Product/getAllProducts");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<MessageResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal("no products found", responseContent.Message);
        }


        [Fact]
        public async Task UpdateProduct_ValidProduct_ReturnsOkWithMessage()
        {
            // Arrange
            var initialProduct = new Product
            {
                Id = 1,
                Name = "testowy",
                Ean = "12345",
                Stock = 10,
                sku = "SKU-001",
                Category = new Category { Id = 1, Name = "test kategoria" }
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

      
                dbContext.Products.RemoveRange(dbContext.Products);
                await dbContext.SaveChangesAsync();

  
                dbContext.Categories.Add(initialProduct.Category);

                dbContext.Products.Add(initialProduct);
                await dbContext.SaveChangesAsync();
            }

            var updatedProduct = new Product
            {
                Id = 1,
                Name = "testowy updated",
                Ean = "54321",
                Stock = 20,
                sku = "SKU-002",
                Category = new Category { Id = 1, Name = "test kategoria" }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Product/updateProduct", updatedProduct);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Update Response: {response.StatusCode}, Body: {responseBody}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

 
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var product = await dbContext.Products.FindAsync(1);
                Assert.NotNull(product);
                Assert.Equal("testowy updated", product.Name);
            }
        }

        internal class ErrorResponse
        {
            public string Error { get; set; }
        }

        internal class MessageResponse
        {
            public string Message { get; set; }
        }
    }
}