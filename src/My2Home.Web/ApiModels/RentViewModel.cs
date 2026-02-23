using My2Home.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My2Home.Web.ApiModels
{
    public class RentViewModel: BaseViewModel
    {
        public int RentId { get; set; }

        public string RentMonth { get; set; }
        public int RentYear { get; set; }
        public string RentStatus { get; set; }
        public int RentAmount { get; set; }
        public int RentBedNumber { get; set; }
        public DateTime RentCreationDate { get; set; } = DateTime.UtcNow;
        public DateTime RentDueDateTime { get; set; }
        public int RentHostelId { get; set; }
        public int RentTenantId { get; set; }
        [Required]
        public DateTime RentDateTime { get; set; }
    }

    public class RentListViewModel : BaseViewModel
    {
        public int RentId { get; set; }
        public string RentMonth { get; set; }
        public int RentYear { get; set; }
        public string RentStatus { get; set; }
        public int RentAmount { get; set; }
        public int RentBedNumber { get; set; }
        public DateTime RentCreationDate { get; set; } = DateTime.UtcNow;
        public DateTime RentDueDateTime { get; set; }
        public int RentHostelId { get; set; }
        public int RentTenantId { get; set; }
        public DateTime RentDateTime { get; set; }
        public TenantViewModel RentTenant { get; set; }
}

    public class RentCreateViewModel : BaseViewModel
    {
        public int RentId { get; set; }
        public string RentMonth { get; set; }
        public int RentYear { get; set; }
        public string RentStatus { get; set; }
        public int RentAmount { get; set; }
        public int RentBedNumber { get; set; }
        public DateTime RentCreationDate { get; set; } = DateTime.UtcNow;
        public DateTime RentDueDateTime { get; set; }
        public int RentHostelId { get; set; }
        public int RentTenantId { get; set; }
        public DateTime RentDateTime { get; set; }

    }

}
