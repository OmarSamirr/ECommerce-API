using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DataTransferObjects.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> Create(OrderRequest orderRequest)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.OrderService.CreateAsync(orderRequest, email));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResponse>> Get([FromRoute] Guid id)
        {
            return Ok(await _serviceManager.OrderService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> Get()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.OrderService.GetAllAsync(email));
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResponse>>> GetDeliveryMethods()
        {
            return Ok(await _serviceManager.OrderService.GetDeliveryMethodsAsync());
        }
    }
}
