using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("Tenant")]
    public  class TenantEntity: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TenantId { get; set; }
        public string TenantName { get;set;}
        public string TenantMobile { get;set;}
        public string TenantEmail { get;set;}       
        public string TenantIDType { get; set; }
        //public string Image {get;set;}
        public string TenantHomeAddress { get; set; }
        public string TenantEmergencyPerson { get; set; }
        public string TenantEmergencyContact { get; set; }
        public int TenantBedId { get;set;}
        public string TenantProfession { get;set;}
        public string TenantWorkInstitutionAddress { get; set; }
        public string TenantStatus { get;set;}
        public string TenantExtraNote  { get; set; }
        //public string UpdatedOn { get; set; }
        public DateTime TenantUpdatedOn { get; set; }
        public int TenantAdvanceDepositSecurity { get;set;}
        public string TenantVehicleNumber { get; set; }
       
        public int? TenantIdentityUserId { get; set; }
        public int TenantHostelId { get; set; }
        
        [NotMapped]
        public string RoomNumber { get; set; }
        [NotMapped]
        public int TotalRows { get; set; }

        public BedEntity TenantBed { get; set; }
    }
}
