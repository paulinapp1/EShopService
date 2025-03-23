
using EShop.Domain.Exceptions.CardNumber;

namespace Eshop.Application.Tests
{
    public class CreditCardServiceTest
    {
        [Theory]
        [InlineData("4539 1488 0343 6467")]
        [InlineData("4024-0071-6540-1778")]
        [InlineData("345-470-784-783-010")]
        [InlineData("4532 2080 2150 4434")]
        [InlineData("5551561443896215")]


        
        public void ValidateCard_CardNumber_ShouldReturnTrue(string cardNum)
        {
            //Arrange
            var card = new CreditCardService();
       
            // Act & Assert
           var result= card.ValidateCard(cardNum);
            Assert.True(result);
        }
        [Theory]
        [InlineData("123433333333333333333333333")]
        [InlineData("111111111111111111111111111111111")]
        [InlineData("1444455666666666666666666666")]
        [InlineData("566&438283%%83u4u49m6666666666")]
       
        public void ValidateCard_CardNumber_tooLong(string cardNumber)
        {
            //Arrange
            var card = new CreditCardService();

            // Act & Assert
            var exception = Assert.Throws<CardNumberTooLongException>(() => card.ValidateCard(cardNumber));
            Assert.Equal("Card number is too long", exception.Message);
        }
        [Theory]
        [InlineData("5")]
        [InlineData("6666")]
        [InlineData("5555555")]

        public void ValidateCard_CardNumber_tooShort(string cardNumber)
        {
            //Arrange
            var card = new CreditCardService();

            // Act & Assert
            var exception = Assert.Throws<CardNumberTooShortException>(() => card.ValidateCard(cardNumber));
            Assert.Equal("Card number is too short", exception.Message);
        }
        [Theory]
        [InlineData("3497 7965 8312 797", "American Express")]
        [InlineData("378523393817437", "American Express")]
        [InlineData("4024-0071-6540-1778", "Visa")]
        [InlineData("4532 2080 2150 4434", "Visa")]
        [InlineData("5530016454538418", "MasterCard")]
        [InlineData("5131208517986691", "MasterCard")]
        
        public void GetCardType_should_return_correct(string CardNumber, string CardType)
        {
            //Arrange
            var card = new CreditCardService();
            //Act
            var result = card.GetCardType(CardNumber, CardType);
            //Assert
            Assert.Equal(CardType, result);
        }
        [Theory]
        [InlineData("3497 7965 8312 797", "MasterCard")]
        [InlineData("345-470-784-783-010", "Visa")]
        [InlineData("5530016454538418", "American Express")]
        [InlineData("4024-0071-6540-1778", "MasterCard")]
        [InlineData("4532289052809181", "American Express")]
        public void GetCardType_InvalidCardNumber_ThrowsException(string cardNumber, string CardType)
        {
            //Arrange
           var card = new CreditCardService();
            //Act
            var exception = Assert.Throws<CardNumberInvalidException>(() => card.GetCardType(cardNumber, CardType));
            //Assert
           Assert.Equal("Card number is invalid", exception.Message);

       }
        
    }
}