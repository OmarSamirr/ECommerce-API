using Shared.DataTransferObjects.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface IProductService
    {
        //get all
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        //get product by id
        Task<ProductResponse> GetProductByIdAsync(int id);
        //get all brands
        Task<IEnumerable<BrandResponse>> GetAllBrandsAsync();
        //get all types
        Task<IEnumerable<TypeResponse>> GetAllTypesAsync();

    }
}
