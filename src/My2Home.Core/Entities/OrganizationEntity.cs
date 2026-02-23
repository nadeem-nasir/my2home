using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("Organization")]
    public class OrganizationEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganizationId {get;set;}
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationPhoneNumber { get; set; }

        [NotMapped]
        public int TotalRows { get; set; }
    }

   public class OrganizationIdentityUser
   {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool IsLock
        {
            get
            {
              return  LockoutEnabled && LockoutEnd.HasValue && LockoutEnd > DateTimeOffset.UtcNow ? true : false;
            }
            set
            {
                value = LockoutEnabled && LockoutEnd.HasValue && LockoutEnd > DateTimeOffset.UtcNow ? true : false;
            }
        }

    }
}
