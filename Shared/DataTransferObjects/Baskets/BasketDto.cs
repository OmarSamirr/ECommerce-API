using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.Baskets
{
    public class BasketDto
    {
        public string Id { get; set; } = default!;//GUID
        public IEnumerable<BasketItemDto> Items { get; set; } = default!;
    }
}
