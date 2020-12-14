using MediatR;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi1.Contracts.Dto;

namespace WebApi1.Services
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly ProductsDbContext _context;
        public GetProductByIdQueryHandler(ProductsDbContext context)
        {
            _context = context;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = _context.Products.Where(a => a.Id == query.Id).Select(p=>new ProductDto {Id=p.Id, Name=p.Name }).FirstOrDefault();
            if (product == null) return null;
            return product;
        }
    }
}
