using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("OrganizationUsers")]
    public class OrganizationUserEntity: BaseEntity
    {       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationIdentityUserId { get; set; }
    }
}
