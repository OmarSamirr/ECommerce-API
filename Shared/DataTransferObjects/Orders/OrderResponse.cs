using Shared.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = default!;
        public List<OrderItemDto> Items { get; set; } = [];
        public AddressDto Address { get; set; } = default!;
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal SubTotal { get; set; } = default!;
        public decimal Total { get; set; } = default!;
        public DateTimeOffset Date { get; set; }
        public string PaymentStatus { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
    }

    public class OrderItemDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
