using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Specifications;
using ServicesAbstraction;
using Shared;
using Shared.DataTransferObjects.Products;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResponse>> GetAllBrandsAsync()
        {
            var repository = _unitOfWork.GetRepository<ProductBrand, int>();
            var brands = await repository.GetAllAsync();
            var brandsResponse = _mapper.Map<IEnumerable<BrandResponse>>(brands);
            return brandsResponse;
        }

        public async Task<PaginatedResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters productQueryParameters)
        {
            var specs = new ProductWithTypeAndBrandSpecifications(productQueryParameters);//type and brand filter, sorting
            var countSpecs = new ProductsCountSpecifications(productQueryParameters);//type and brand filter, sorting

            var repository = _unitOfWork.GetRepository<Product, int>();

            var products = await repository.GetAllAsync(specs);
            var productCount = await repository.CountAsync(countSpecs);
            var productResopnse = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return new PaginatedResponse<ProductResponse>
            {
                Data = productResopnse,
                PageIndex = productQueryParameters.PageIndex,
                PageSize = productQueryParameters.PageSize,
                Count = productCount
            };
        }

        public async Task<IEnumerable<TypeResponse>> GetAllTypesAsync()
        {
            var repository = _unitOfWork.GetRepository<ProductType, int>();
            var types = await repository.GetAllAsync();
            var typesResponse = _mapper.Map<IEnumerable<TypeResponse>>(types);
            return typesResponse;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var specs = new ProductWithTypeAndBrandSpecifications(id);//filter with id

            var repository = _unitOfWork.GetRepository<Product, int>();
            var product = await repository.GetByIdAsync(specs);
            return _mapper.Map<ProductResponse>(product);
        }
    }
}
