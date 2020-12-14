using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApi1.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class SettingsController : ControllerBase
    {
        IConfiguration _configuration;

        public SettingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<string[]> GetSettings()
        {
            List<string> list = new List<string>();
           var sectionRoot =  _configuration.GetSection("BackgroundService");
            foreach (IConfigurationSection section in sectionRoot.GetChildren())
            {
                var key = section.GetValue<string>("Name");
                var keyStart = section.GetValue<string>("Autostart");
                var value = section.GetValue<string>("Time");
                list.Add(key);
            }

            return list.ToArray();

        }
    }
}