using AspNet.Security.OpenIdConnect.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace My2Home.Web.Extensions
{
    public static  class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            var id = principal.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;
            return id;
        }



        public static int GetUserCountryId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            if (!principal.HasClaim(c => c.Type == "My2Home:CountryId"))
            {
                return 1;
            }
            var id = principal.FindFirst("My2Home:CountryId")?.Value;
            return Convert.ToInt32(id);
        }
        public static int GetUserOrganizationId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));           
            var id = principal.FindFirst("My2Home:OrganizationId")?.Value;
            return Convert.ToInt32(id);
        }

        public static DateTime GetUserExpiryDateTime(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            var id = principal.FindFirst("My2Home:ExpiryDateTime")?.Value;
            return Convert.ToDateTime(id);
        }
    }   
}
