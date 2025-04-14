using EShopService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context=context; 
        }
        public void AddProduct(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null) {
                _context.Remove(product);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Find(id);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
