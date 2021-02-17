using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebApi1.Contracts.Dto;

namespace WebApi1.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Bearer")]
  
    public class ProductsController : ControllerBase
    {
        private IMediator _mediator;
      
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;       
        }



     
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var result =await _mediator.Send(new GetAllProductsQuery());

            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductDto>> GetProducts2()
        {
            var result = new[] { new ProductDto { Id = 1, Name = "First" }, new ProductDto { Id = 2, Name = "Second" } };

            return result;
        }

        [HttpGet]
        public async Task<ProductDto> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });

            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<int> Create(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);

            return result;
        }

        [HttpGet]
        public async Task<string> GetUser()
        {
            var name =  User.Identity.Name;
            var email = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            return name;
        }
    }
}