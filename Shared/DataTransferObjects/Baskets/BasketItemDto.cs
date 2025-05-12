using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.Baskets
{
    public class BasketItemDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        [Range(1, byte.MaxValue)]
        public int Quantity { get; set; }
        [Range(1, 99_999)]
        public decimal Price { get; set; }
    }
}
