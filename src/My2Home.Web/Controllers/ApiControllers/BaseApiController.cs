using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using My2Home.Web.Identity;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace My2Home.Web.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseApiController<T> : ControllerBase where T:BaseApiController<T>
    {
        private  ILogger<T> __logger;
        private  UserManager<ApplicationIdentityUser> __userManager;
        private IMapper _mapper;
        protected ILogger<T> _logger => __logger ?? (__logger = HttpContext?.RequestServices.GetService<ILogger<T>>());
        protected UserManager<ApplicationIdentityUser> _userManager => __userManager ?? (__userManager = HttpContext?.RequestServices.GetService<UserManager<ApplicationIdentityUser>>());


        protected IMapper GetMapper => _mapper ?? (_mapper = HttpContext?.RequestServices.GetService<IMapper>());
    }
}