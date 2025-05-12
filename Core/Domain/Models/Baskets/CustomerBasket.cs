using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Baskets
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!;//GUID: Generated From Client Side
        public IEnumerable<BasketItem> Items { get; set; } = default!;
    }
}
