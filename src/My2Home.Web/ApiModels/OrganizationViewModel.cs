using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace My2Home.Web.ApiModels
{
    
    public class OrganizationViewModel : BaseViewModel
    {
        public int OrganizationId {get;set;}
        [MaxLength(200)]
        public string OrganizationName { get; set; }
        [MaxLength(1000)]
        public string OrganizationAddress { get; set; }
        [MaxLength(50)]
        public string OrganizationPhoneNumber { get; set; }
    }
}
