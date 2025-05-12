﻿using Shared.DataTransferObjects.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface IBasketService
    {
        Task<BasketDto> GetAsync(string id);
        Task<BasketDto> UpdateAsync(BasketDto basketDto);
        Task DeleteAsync(string id);
    }
}
