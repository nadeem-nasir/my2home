using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    public class AccountViewModel
    {
        public int RentHostelId { get; set; }
        public int RentYear { get; set; }
        public string RentMonth { get; set; }        
        public int TotalRents { get; set; }
        public int PaidRents { get; set; }
        public int DueRents { get; set; }
        public int TotalExpenses { get; set; }
        [NotMapped]
        public int TotalRows { get; set; }
    }
}
