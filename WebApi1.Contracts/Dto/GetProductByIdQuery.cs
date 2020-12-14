using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi1.Contracts.Dto
{
    // возвращаемый тип - ProductDto,
    // Handler - в друой сборке (по идее в Infrastructure)
    public class GetProductByIdQuery : IRequest<ProductDto> 
    {
        public int Id { get; set; }
        
    }
}
