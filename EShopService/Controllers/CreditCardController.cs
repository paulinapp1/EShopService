using Eshop.Application;
using EShop.Domain.Enums;
using EShop.Domain.Exceptions.CardNumber;
using EShopService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardService creditCardService;
        private readonly IProductService productService;

        public CreditCardController(ICreditCardService _creditCardService, IProductService _productService)
        {
            creditCardService = _creditCardService;
            productService = _productService;
        }
        // GET: api/<CreditCardController>
        [HttpGet("verifyCreditCard")]
        public IActionResult VerifyCard([FromQuery] string cardNumber)
        {
            try
            {
                bool isValid = creditCardService.ValidateCard(cardNumber);

                string cardType = creditCardService.GetCardType(cardNumber);
                if (Enum.TryParse<CreditCardProvider>(cardType, true, out _))
                {
                    return Ok(new { IsValid = isValid, CardType = cardType });
                }
                else
                {
                    return StatusCode(406, new { Error = "Card type not supported" });
                }
            }
            catch (CardNumberTooShortException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (CardNumberTooLongException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (CardNumberInvalidException ex)
            {
                return StatusCode(400, new { Error = ex.Message });
            }
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
            catch (Exception ex) {
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
            catch (Exception ex) {
                return StatusCode(500, new { Error = "an error occurred while updating the product" });
            }

        }
    }
}
