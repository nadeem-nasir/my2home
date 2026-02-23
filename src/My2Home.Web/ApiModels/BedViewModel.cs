using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace My2Home.Web.ApiModels
{
   
    public class BedViewModel : BaseViewModel
    {
        public int BedNumber { get; set; }
        public int RoomId { get; set; }
        public int BedRent { get; set; }
        [MaxLength(500)]
        public string BedRemarks { get; set; }
        public string BedStatus { get; set; }
    }
}
