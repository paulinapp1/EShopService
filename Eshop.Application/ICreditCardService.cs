using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application
{
    public interface ICreditCardService
    {
        public string GetCardType(string cardNumber, string expectedCardType=null);
        public bool ValidateCard(string cardNumber);
    }
}
