using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("Rent")]
    public class RentEntity:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RentId { get; set; }
        public string RentMonth { get; set; }
        public int RentYear { get; set; }
        public string RentStatus { get; set; }
        public int RentAmount { get; set; }
        public int RentBedNumber { get; set; }
        public DateTime RentCreationDate { get; set; }
        public DateTime RentDueDateTime { get; set; }
        public DateTime RentDateTime { get; set; }
        public int RentHostelId { get; set; }
        public int RentTenantId { get; set; }

        public TenantEntity RentTenant { get; set; }
        [NotMapped]
        public int TotalRows { get; set; }

    }
}
