﻿using Domain.Models.Products;
using Shared.DataTransferObjects.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductsCountSpecifications(ProductQueryParameters productQueryParameters)
        : BaseSpecifications<Product>(CreateCriteria(productQueryParameters))
    {
        private static Expression<Func<Product, bool>> CreateCriteria(ProductQueryParameters productQueryParameters)
        {
            return prod =>
                    (!productQueryParameters.BrandId.HasValue || prod.BrandId == productQueryParameters.BrandId.Value) &&
                    (!productQueryParameters.TypeId.HasValue || prod.TypeId == productQueryParameters.TypeId.Value) &&
                    (string.IsNullOrWhiteSpace(productQueryParameters.SearchKeyword) || prod.Name.ToLower().Contains(productQueryParameters.SearchKeyword.ToLower()));
        }
    }
}
