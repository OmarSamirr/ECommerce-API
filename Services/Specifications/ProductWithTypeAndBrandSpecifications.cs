using Domain.Models;
using Shared.DataTransferObjects.Products;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product>
    {
        //used to get product by id
        public ProductWithTypeAndBrandSpecifications(int id) : base(prod => prod.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        //used to get all products
        public ProductWithTypeAndBrandSpecifications(ProductQueryParameters productQueryParameters) : base(CreateCriteria(productQueryParameters))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            ApplySorting(productQueryParameters);
            AddPagination(productQueryParameters.PageSize, productQueryParameters.PageIndex);
        }

        private static Expression<Func<Product, bool>> CreateCriteria(ProductQueryParameters productQueryParameters)
        {
            return prod =>
                    (!productQueryParameters.BrandId.HasValue || prod.BrandId == productQueryParameters.BrandId.Value) &&
                    (!productQueryParameters.TypeId.HasValue || prod.TypeId == productQueryParameters.TypeId.Value) &&
                    (string.IsNullOrWhiteSpace(productQueryParameters.SearchKeyword) || prod.Name.ToLower().Contains(productQueryParameters.SearchKeyword.ToLower()));
        }
        private void ApplySorting(ProductQueryParameters productQueryParameters)
        {
            switch (productQueryParameters.ProductSortingOptions)
            {
                case ProductSortingOptions.NameAscending:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDescending:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAscending:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDescending:
                    AddOrderByDescending(p => p.Price);
                    break;
            }
        }
    }
}
