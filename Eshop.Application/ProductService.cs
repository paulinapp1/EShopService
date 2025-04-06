using EShop.Domain.Repositories;
using EShopService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Product GetProduct(int id)
        {
            return _productRepository.GetProductById(id);

        }
        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public void AddProduct(Product product) { 
             _productRepository.AddProduct(product);
        }
        public bool DeleteProduct(int id)
        {
           return _productRepository.DeleteProduct(id);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.UpdateProduct(product);
        }

      
    }
}
