using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace My2Home.Web.Identity
{
    public class ApplicationIdentityUser : IdentityUser
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }        
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? LastLoginDateTime { get; set; }
        public virtual DateTime? ExpiryDateTime { get; set; }
        public virtual int? CountryId { get; set; }
        public virtual string PlanTextPassword { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        [NotMapped]
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;

        

    }
}
