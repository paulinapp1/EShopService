using Eshop.Application;
using EShop.Domain.Enums;
using EShop.Domain.Exceptions.CardNumber;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace EShopService.Tests.Integration
{
    public class CreditCardControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<ICreditCardService> mockCreditCardService;

        public CreditCardControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            mockCreditCardService = new Mock<ICreditCardService>();

        }

        [Fact]
        public async Task VerifyCard_ValidCard_ReturnsOkWithCardType()
        {
            // Arrange
            
            string validCardNumber = "4532 2080 2150 4434";
            mockCreditCardService.Setup(s => s.ValidateCard(validCardNumber)).Verifiable();
            mockCreditCardService.Setup(s => s.GetCardType(validCardNumber)).Returns(CreditCardProvider.Visa);

            var client = CreateClientWithMockedService(mockCreditCardService);

            // Act
            var response = await client.GetAsync($"/api/CreditCard/verifyCreditCard?cardNumber={validCardNumber}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<CardTypeResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal(CreditCardProvider.Visa, responseContent.CardProvider);

            mockCreditCardService.Verify(s => s.ValidateCard(validCardNumber), Times.Once);
            mockCreditCardService.Verify(s => s.GetCardType(validCardNumber), Times.Once);
        }

        [Fact]
        public async Task VerifyCard_TooShortCardNumber_ReturnsBadRequest()
        {
            // Arrange
      
            string shortCardNumber = "11";
            string errorMessage = "Card number is too short";
            mockCreditCardService.Setup(s => s.ValidateCard(shortCardNumber))
                .Throws(new CardNumberTooShortException(errorMessage));

            var client = CreateClientWithMockedService(mockCreditCardService);

            // Act
            var response = await client.GetAsync($"/api/CreditCard/verifyCreditCard?cardNumber={shortCardNumber}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal(errorMessage, responseContent.Error);

            mockCreditCardService.Verify(s => s.ValidateCard(shortCardNumber), Times.Once);
        }

        [Fact]
        public async Task VerifyCard_TooLongCardNumber_ReturnsNotFound()
        {
            // Arrange
       
            string longCardNumber = "11111111111111111111111111111";
            string errorMessage = "Card number is too long";
            mockCreditCardService.Setup(s => s.ValidateCard(longCardNumber))
                .Throws(new CardNumberTooLongException(errorMessage));

            var client = CreateClientWithMockedService(mockCreditCardService);

            // Act
            var response = await client.GetAsync($"/api/CreditCard/verifyCreditCard?cardNumber={longCardNumber}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal(errorMessage, responseContent.Error);

            mockCreditCardService.Verify(s => s.ValidateCard(longCardNumber), Times.Once);
        }

        [Fact]
        public async Task VerifyCard_InvalidCardNumber_ReturnsBadRequest()
        {
            // Arrange
         
            string invalidCardNumber = "1234567890123456";
            string errorMessage = "Card number is invalid";
            mockCreditCardService.Setup(s => s.ValidateCard(invalidCardNumber))
                .Throws(new CardNumberInvalidException(errorMessage));

            var client = CreateClientWithMockedService(mockCreditCardService);

            // Act
            var response = await client.GetAsync($"/api/CreditCard/verifyCreditCard?cardNumber={invalidCardNumber}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
Assert.Contains(errorMessage, responseContent);


            mockCreditCardService.Verify(s => s.ValidateCard(invalidCardNumber), Times.Once);
        }


        [Fact]
        public async Task VerifyCard_VisaCardType_ReturnsCorrectCardType()
        {
            // Arrange
        
            string cardNumber = "4532 2080 2150 4434";
            CreditCardProvider expectedCardType = CreditCardProvider.Visa;
            mockCreditCardService.Setup(s => s.ValidateCard(cardNumber)).Verifiable();
            mockCreditCardService.Setup(s => s.GetCardType(cardNumber)).Returns(expectedCardType);

            var client = CreateClientWithMockedService(mockCreditCardService);

            // Act
            var response = await client.GetAsync($"/api/CreditCard/verifyCreditCard?cardNumber={cardNumber}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadFromJsonAsync<CardTypeResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal(expectedCardType, responseContent.CardProvider);
        }

        [Fact]
        public async Task VerifyCard_MasterCardType_ReturnsCorrectCardType()
        {
            // Arrange
          
            string cardNumber = "5530016454538418";
            CreditCardProvider expectedCardType = CreditCardProvider.Mastercard;
            mockCreditCardService.Setup(s => s.ValidateCard(cardNumber)).Verifiable();
            mockCreditCardService.Setup(s => s.GetCardType(cardNumber)).Returns(expectedCardType);

            var client = CreateClientWithMockedService(mockCreditCardService);

            // Act
            var response = await client.GetAsync($"/api/CreditCard/verifyCreditCard?cardNumber={cardNumber}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadFromJsonAsync<CardTypeResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal(expectedCardType, responseContent.CardProvider);
        }

        [Fact]
        public async Task VerifyCard_AmexCardType_ReturnsCorrectCardType()
        {
            // Arrange
        
            string cardNumber = "3497 7965 8312 797";
            CreditCardProvider expectedCardType = CreditCardProvider.AmericanExpress;
            mockCreditCardService.Setup(s => s.ValidateCard(cardNumber)).Verifiable();
            mockCreditCardService.Setup(s => s.GetCardType(cardNumber)).Returns(expectedCardType);

            var client = CreateClientWithMockedService(mockCreditCardService);

            // Act
            var response = await client.GetAsync($"/api/CreditCard/verifyCreditCard?cardNumber={cardNumber}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadFromJsonAsync<CardTypeResponse>();
            Assert.NotNull(responseContent);
            Assert.Equal(expectedCardType, responseContent.CardProvider);
        }

        
       
        private HttpClient CreateClientWithMockedService(Mock<ICreditCardService> mockCreditCardService)
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(_ => mockCreditCardService.Object);
                });
            }).CreateClient();
        }
    }


    internal class CardTypeResponse
    {
        public CreditCardProvider CardProvider { get; set; }
    }

    internal class ErrorResponse
    {
        public string Error { get; set; }
    }
}