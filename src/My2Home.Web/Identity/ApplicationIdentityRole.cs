using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My2Home.Web.Identity
{
    public class ApplicationIdentityRole : IdentityRole<int>
    {
        //[StringLength(250)]
        //public string Description { get; set; }

    }
}
