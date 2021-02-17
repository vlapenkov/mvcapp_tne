using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mvcapp.Models;

namespace mvcapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
           // some home work
           var valuesSection = _configuration.GetSection("Menu:Items");
            foreach (IConfigurationSection section in valuesSection.GetChildren())
            {
                var key = section.GetValue<string>("Key");
                var value = section.GetValue<string>("Value");
                _logger.LogInformation($"key: {key} value:{value}");
            }
            _logger.LogWarning($"Some warning from {nameof(HomeController)}");
            return View();
        }

        public IActionResult Privacy()
        {

          var options =  new CookieOptions
            {
                // Set the secure flag, which Chrome's changes will require for SameSite none.
                // Note this will also require you to be running on HTTPS.
                Secure = false,

                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                HttpOnly = true,

                // Add the SameSite attribute, this will emit the attribute with a value of none.
                // To not emit the attribute at all set
                // SameSite = (SameSiteMode)(-1)
                SameSite = SameSiteMode.None
            };
            HttpContext.Response.Cookies.Append("name", "Tom1", options);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Restricted()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // return new SignOutResult(new[] { "oidc", "Cookies" });
            return SignOut("Cookies", "oidc");
        }

        
    }
}
