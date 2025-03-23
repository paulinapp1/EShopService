using Eshop.Application;
using EShop.Domain.Exceptions.CardNumber;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly CreditCardService creditCardService;

        public CreditCardController(CreditCardService _creditCardService)
        {
            creditCardService = _creditCardService;
        }
        // GET: api/<CreditCardController>
        [HttpGet("verifyCreditCard")]
        public IActionResult VerifyCard([FromQuery] string cardNumber)
        {
            try
            {
                bool isValid = creditCardService.ValidateCard(cardNumber);
                string cardType = creditCardService.GetCardType(cardNumber);
                return Ok(new { IsValid = isValid, CardType = cardType });
            }
            catch (CardNumberTooShortException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (CardNumberTooLongException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (CardNumberInvalidException ex) {
                return StatusCode(406, new { Error = ex.Message });
            }
        }

       
    }
}
