using Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class OrderSpecifications : BaseSpecifications<Order>
    {
        //For Get By Id
        public OrderSpecifications(Guid id) : base(o => o.Id == id)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
        }
        //For Get All
        public OrderSpecifications(string email) : base(o => o.UserEmail == email)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
