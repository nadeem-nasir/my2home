using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Web.ApiModels
{
    
    public class CountryViewModel : BaseViewModel
    {
        
        public int CountryId { get; set; }
        [MaxLength(200)]
        [Required]
        public string CountryName { get; set; }
        public string CountryTwoLetterIsoCode { get; set; }
        public string CountryThreeLetterIsoCode { get; set; }
        public string CountryNumericIsoCode { get; set; }
        public string CountrySubjectToVat { get; set; }
        public string CountryPublished { get; set; }
        public string CountrySortOrder { get; set; }
        public string CountryCurrencyName { get; set; }
        public string CountryPhoneCode { get; set; }

    }
}
