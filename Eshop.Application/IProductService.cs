using EShopService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application
{
    public interface IProductService
    {
        public Product GetProduct(int id);
        public bool DeleteProduct(int id);
        public void UpdateProduct(Product product);
        public IEnumerable<Product> GetProducts();
        public void AddProduct(Product product);
    }
}
