using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("Beds")]
    public class BedEntity: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BedNumber { get; set; }
        public int RoomId { get; set; }       
        public int  BedRent { get; set; }
        public string BedRemarks {get;set;}
        public string BedStatus { get; set; }
        [NotMapped]
        public int TotalRows { get; set; }
        [NotMapped]
        public string RoomNumber { get; set; }

    }
}
