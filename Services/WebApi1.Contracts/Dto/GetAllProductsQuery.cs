using MediatR;
using System;
using System.Collections.Generic;
using System.Text;


namespace WebApi1.Contracts.Dto
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
       
    }
}
