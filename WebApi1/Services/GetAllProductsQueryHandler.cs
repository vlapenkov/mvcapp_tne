using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi1.Contracts.Dto;

namespace WebApi1.Services
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly ProductsDbContext _context;
        public GetAllProductsQueryHandler(ProductsDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
        {
            var productList = await _context.Products
                .Select(product=> new ProductDto 
                {
                    Id=product.Id,
                    Name=product.Name 
                })
                .ToListAsync();
            if (productList == null)
            {
                return null;
            }
            return productList.AsReadOnly();
        }
    }
}
