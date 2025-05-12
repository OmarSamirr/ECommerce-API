using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BasketNotFoundException(string basketKey)
                 : NotFoundException($"Basket With this Id : {basketKey} is not found.")
    {
    }
}
