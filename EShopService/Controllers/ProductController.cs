using Eshop.Application;
using EShopService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController( IProductService _productService)
        {
            productService = _productService;
        }
        [HttpGet("getProductByID")]
        public IActionResult GetProductByID([FromQuery] int productID)
        {
            try
            {
                var product = productService.GetProduct(productID);
                if (product == null)
                {
                    return NotFound(new { Error = "product not found" });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = "prodcut  not found" });
            }

        }
        [HttpGet("deleteProductByID")]
        public IActionResult DeleteProductByID([FromQuery] int productID)
        {
            try
            {
                bool deleted = productService.DeleteProduct(productID);
                if (deleted)
                {
                    return Ok();
                }
                return NotFound(new { Error = "product not found" });
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = "product not found" });
            }

        }
        [HttpGet("getAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = productService.GetProducts();
                if (!products.Any())
                {
                    return Ok(new { Message = "no products found" });

                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "an error occurred while retrieving products" });
            }
        }
        [HttpPost("addProduct")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest(new { Error = "product data is invalid" });
                }
                productService.AddProduct(product);


                return CreatedAtAction(nameof(GetProductByID), new { productID = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "an error occurred while adding the product" });
            }
        }
        [HttpPost("updateProduct")]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest(new { Error = "product data is invalid" });
                }
                productService.UpdateProduct(product);
                return Ok(new { Message = "product updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "an error occurred while updating the product" });
            }

        }
    }
}