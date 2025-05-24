using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DataTransferObjects.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]//BaseUrl/api/Baskets
    [ApiController]
    [Authorize]
    public class BasketsController(IServiceManager _serviceManager) : ControllerBase
    {
        //Get User Basket
        [HttpGet]
        public async Task<ActionResult<BasketDto>> Get(string id)
        {
            var basket = await _serviceManager.BasketService.GetAsync(id);
            return Ok(basket);
        }

        //Update User Basket
        //create Basket
        //Add Item To Basket
        //Remove Item From Basker
        //update Items Quantity
        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basketDto)
        {
            var basket = await _serviceManager.BasketService.UpdateAsync(basketDto);
            return Ok(basket);
        }

        //Delete User Basket
        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            await _serviceManager.BasketService.DeleteAsync(id);
            return NoContent();
        }
    }
}
