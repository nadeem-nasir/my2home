using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("Hostel")]
    public class HostelEntity: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  HostelId {get;set;}
        public string HostelName {get;set;}
        public string HostelAddress {get;set;}
        public string HostelContactNumber {get;set;}
        public string HostelContactNumber2 {get;set;}
        public string HostelContactPersonName {get;set;}
        public string HostelContactPersonName2 {get;set;}
        public string HostelLat { get;set;}
        public string HostelLong { get;set;}
        public int HostelOrganizationId { get;set;}
        public int HostelCityId { get; set; }

        [NotMapped]
        public int TotalRows { get; set; }

    }
}
