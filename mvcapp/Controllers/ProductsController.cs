using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
using WebApi1.Contracts.Dto;
using WebApi1.Contracts.Interfaces;

namespace mvcapp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var client = new HttpClient();
            //client.SetBearerToken(accessToken);
            //var response = await client.GetStringAsync("http://localhost:5100/api/Products/GetProducts");
            //return Ok(response);




            var products = await _productService.GetProducts();


            return View(products);
        }
    }
}