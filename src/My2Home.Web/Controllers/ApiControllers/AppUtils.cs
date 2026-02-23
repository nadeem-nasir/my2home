using Microsoft.AspNetCore.Mvc;
using My2Home.Web.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My2Home.Web.Controllers.ApiControllers
{
    public class AppUtils
    {
        internal static IActionResult SignIn(ApplicationIdentityUser user, IList<string> roles)
        {
            var userResult = new { User = new { DisplayName = user.UserName, Roles = roles } };
            return new ObjectResult(userResult);
        }


    }
}
