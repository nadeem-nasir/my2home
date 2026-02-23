using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace My2Home.Web.ApiModels
{
    
    public class HostelViewModel: BaseViewModel
    {
        public int HostelId { get; set; }
        [MaxLength(200)]
        public string HostelName { get; set; }
        [MaxLength(1000)]
        public string HostelAddress { get; set; }
        [MaxLength(50)]
        public string HostelContactNumber { get; set; }
        [MaxLength(50)]
        public string HostelContactNumber2 { get; set; }
        [MaxLength(200)]
        public string HostelContactPersonName { get; set; }
        [MaxLength(200)]
        public string HostelContactPersonName2 { get; set; }
        public string HostelLat { get; set; }
        public string HostelLong { get; set; }
        public int HostelOrganizationId { get; set; }
        public int HostelCityId { get; set; }
    }
}
