using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using My2Home.Core.AppSettings;

namespace My2Home.Web.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private readonly AppSettings _AppSettings;

        public AppSettingsController(IOptions<AppSettings> appSettings)
        {

            _AppSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_AppSettings.IdentityAuthConfig);
        }

    }
}