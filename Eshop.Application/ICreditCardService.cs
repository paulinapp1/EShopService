using EShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application
{
    public interface ICreditCardService
    {
        public CreditCardProvider GetCardType(string cardNumber);
        public bool ValidateCard(string cardNumber);
    }
}
