using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Mvc;
using My2Home.Web.ViewModels.Shared;

namespace My2Home.Web.Controllers.WebControllers
{
    public class ErrorController : Controller
    {
        [HttpGet, HttpPost, Route("~/error")]
        public IActionResult Error(OpenIdConnectResponse response)
        {
            // If the error was not caused by an invalid
            // OIDC request, display a generic error page.
            if (response == null)
            {
                return View(new ErrorViewModel());
            }

            return View(new ErrorViewModel
            {
                Error = response.Error,
                ErrorDescription = response.ErrorDescription
            });
        }
    }
}