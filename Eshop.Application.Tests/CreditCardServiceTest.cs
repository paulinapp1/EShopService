
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
   
        

        public void ValidateCard_CardNumber_ShouldReturnTrue(string CardNum)
        {
            //Arrange
            var card = new CreditCardService();
            //Act
            var result= card.ValidateCard(CardNum);
            //Assert
            Assert.True(result);
        }
        [Theory]
        [InlineData("1234")]
        [InlineData("111111111111111111111111111111111")]
        [InlineData("14444556")]
        [InlineData("566&438283%%83u4u49")]
        [InlineData("5")]
        public void ValidateCard_CardNumber_ShouldReturnFalse(string CardNum)
        {
            //Arrange
            var card = new CreditCardService();
            //Act
            var result = card.ValidateCard(CardNum);
            //Assert
            Assert.False(result);
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
            var result = card.GetCardType(CardNumber);
            //Assert
            Assert.Equal(CardType, result);
        }
        [Theory]
        [InlineData("3497 7965 8312 797", "MasterCard")]
        [InlineData("345-470-784-783-010", "Visa")]
        [InlineData("5530016454538418", "American Express")]
        [InlineData("4024-0071-6540-1778", "MasterCard")]
        [InlineData("4532289052809181", "American Express")]
        public void GetCardType_should_return_incorrect(string CardNumber, string CardType)
        {
            //Arrange
           var card = new CreditCardService();
            //Act
            var result = card.GetCardType(CardNumber);
            //Assert
           Assert.NotEqual(CardType, result);

       }
        
    }
}