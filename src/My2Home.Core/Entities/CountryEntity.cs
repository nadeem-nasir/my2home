using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("Country")]
    public class CountryEntity: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryTwoLetterIsoCode { get; set; }
        public string CountryThreeLetterIsoCode { get; set; }
        public string CountryNumericIsoCode { get; set; }
        public string CountrySubjectToVat { get; set; }
        public string CountryPublished { get; set; }
        public string CountrySortOrder { get; set; }
        public string CountryCurrencyName { get; set; }
        public string CountryPhoneCode { get; set; }

        [NotMapped]
        public int TotalRows { get; set; }

    }
}
