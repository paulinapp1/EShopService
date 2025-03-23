using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Exceptions.CardNumber
{
    

        public class CardNumberInvalidException : Exception
        {
            public CardNumberInvalidException() : base("Card number is invalid") { }
            public CardNumberInvalidException(string message) : base(message) { }
            public CardNumberInvalidException(string message, Exception innerException) : base(message, innerException) { }


        }

    
}
