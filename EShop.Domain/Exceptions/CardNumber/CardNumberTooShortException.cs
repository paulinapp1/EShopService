using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Exceptions.CardNumber
{
    public class CardNumberTooShortException : Exception
    {
        public CardNumberTooShortException() : base("Card number is too short") { }
        public CardNumberTooShortException(string message) : base(message) { }
        public CardNumberTooShortException(string message, Exception innerException) : base(message, innerException) { }


    }
}
