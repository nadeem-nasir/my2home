using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Web.ApiModels
{

    public class TenantViewModel : BaseViewModel
    {
        public int TenantId { get; set; }
        [MaxLength(250)]
        public string TenantName { get; set; }
        [MaxLength(50)]
        public string TenantMobile { get; set; }
        [MaxLength(100)]
        public string TenantEmail { get; set; }
        [MaxLength(100)]
        public string TenantIDType { get; set; }
        //public string Image {get;set;}
        [MaxLength(500)]
        public string TenantHomeAddress { get; set; }
        [MaxLength(500)]
        public string TenantEmergencyPerson { get; set; }
        [MaxLength(500)]
        public string TenantEmergencyContact { get; set; }
        public string TenantBedId { get; set; }
        [MaxLength(500)]
        public string TenantProfession { get; set; }
        [MaxLength(500)]
        public string TenantWorkInstitutionAddress { get; set; }
        [MaxLength(50)]
        public string TenantStatus { get; set; }
        [MaxLength(500)]
        public string TenantExtraNote { get; set; }
        //public string UpdatedOn { get; set; }
        public DateTime TenantUpdatedOn { get; set; } = DateTime.UtcNow;
        public string TenantAdvanceDepositSecurity { get; set; }
        public string TenantVehicleNumber { get; set; }
        public string ActiveInActive { get; set; }
        public int TenantIdentityUserId { get; set; }
        public int TenantHostelId { get; set; }
        public string RoomNumber { get; set; }
        public string BedAndRoomNumber
        {
            get
            {
                return $"#{TenantBedId} of the room {RoomNumber}";
            }
            set
            {


            }
        }



    }
}
