using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("ExpenseCategory")]
    public class ExpenseCategoryEntity: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName {get;set;}
       

        [NotMapped]
        public int TotalRows { get; set; }
    }
}
