using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi1.Contracts.Dto;

namespace WebApi1.Contracts.Interfaces
{
   public interface IProductService
    {
        [Get("/api/products/getproducts")]
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
