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

        public CreditCardController(ICreditCardService _creditCardService)
        {
            creditCardService = _creditCardService;
        }
        // GET: api/<CreditCardController>
        [HttpGet("verifyCreditCard")]
        public IActionResult VerifyCard([FromQuery] string cardNumber)
        {
            try
            {
                creditCardService.ValidateCard(cardNumber);
                return Ok(new { cardProvider = creditCardService.GetCardType(cardNumber) });

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
       

        }
    }
