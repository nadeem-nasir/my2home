using System;
using System.Collections.Generic;

using System.Text;

namespace My2Home.Web.ApiModels
{
    
    public class OrganizationUserViewModel: BaseViewModel
    {
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationIdentityUserId { get; set; }
    }
}
