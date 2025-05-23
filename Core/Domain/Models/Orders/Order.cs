using Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public List<OrderItem> Items { get; set; } = [];
        public OrderAddress Address { get; set; } = default!;
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal SubTotal { get; set; } = default!;
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
    }
    public enum PaymentStatus
    {
        Pending = 0,
        PaymentReceived = 1,
        PaymentFailed = 2,
    }
}
