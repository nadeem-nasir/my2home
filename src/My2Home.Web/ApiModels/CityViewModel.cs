using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Web.ApiModels
{
    
    public class CityViewModel : BaseViewModel
    {
        public int CityId { get; set; }
        [Required]
        [MaxLength(200)]
        public string CityName { get; set; }
        [Required]
        public int CityCountryId { get; set; }
    }
}
